import os

def main():

    lines = []

    for call in getCalls():
        lines += "\n"+createCSfunc(call)+"\n"

    file = open("glCalls.txt", mode="w")
    file.writelines(lines)

def createCSfunc(signature):

    arguments = signature[signature.find("(")+1:signature.find(")")].split(",")
    newArgs = { }

    for arg in arguments:
        if("." in arg):
            newArgs[arg.split(".")[0].lower()] = arg.split(".")[0]
            print(arg.split(".")[0], arg.split(".")[0].lower())
            print(signature)
            print(arguments)
        elif("ID" in arg):
            newArgs["id"] = "int"
        else:
            try:
                newArgs["i"] = "int"
            except:
                newArgs[""] = arg

    cs_func = "public "+("int" if "Gen" in signature else "void")+" "+signature[signature.find(".")+1:signature.find("(")]+"("
    names = list(newArgs.keys())
    for name in names:
        cs_func += newArgs[name]+" "+name+("," if name != names[-1] else "")

    cs_func += ");"

    return cs_func



def getStarts(file):
    start = 0
    starts = []
    while True:
        index = file.find("GL.", start)
        if(index != -1):
            starts.append(index)
            start = index + 3
        else:
            break

    return starts

def getCalls():
    entries = []
    for dirpath, dirnames, filenames in os.walk("C:/dev/csharp/nephilim"):
        for filename in filenames:
            if(not ".cs" in filename):
                continue
            inp_file = open(dirpath+"/"+filename)
            try:
                file = inp_file.read()
                inp_file.close()
                try:
                    for index in getStarts(file):
                        start_index = index
                        end_index = start_index
                        entry_is_complete = True
                        while(file[end_index] != ";"):
                            end_index += 1
                            if(end_index - start_index > 40):
                                entry_is_complete = False
                                break

                        if(entry_is_complete):
                            entry = file[start_index:end_index]
                            if(not entry in entries):
                                entries.append(entry)
                except:
                    pass
            except:
                continue
    return entries

if __name__ == "__main__":
    main()