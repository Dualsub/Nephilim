import sys
import os


def main():

    if(len(sys.argv) <= 3):
        print("Not enough arguments.")
        return
    
    for path, subdirs, files in os.walk(sys.argv[1]):
        for name in files:
            filepath = path+"/"+name
            r_file = open(filepath, mode="r")
            try:
                content = r_file.read()
                r_file.close()
            except:
                print("Did not open", filepath)
                r_file.close()
                continue
            

            if(sys.argv[2] in content):
                content = content.replace(sys.argv[2], sys.argv[3])
                w_file = open(filepath, mode="w")
                w_file.write(content)
                w_file.close()
                print("Replaced in", name)
            else:
                print("Did not find in", name)

    
if __name__ == "__main__":
    main()



