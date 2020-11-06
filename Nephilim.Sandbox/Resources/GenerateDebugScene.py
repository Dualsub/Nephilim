import sys
import random
import subprocess

def main():

    # Checking for and parsing arguments

    if(len(sys.argv) < 3):
        print("Not enough arguments.")

    print("Setting up...")

    output_path = sys.argv[1]
    numPrefabs = int(sys.argv[2])
    prefabNames = sys.argv[3:]

    # Setting up file

    fileText = '[\n'

    # Adding Player
    
    fileText += '\t{\n\t"Name": "Player"\n\t},\n'

    pos_range = numPrefabs * 20.0

    # Adding random prefabs from random positions

    for i in range(numPrefabs):
        
        index = random.randint(0, len(prefabNames) - 1)

        print(f"Adding {prefabNames[index]}...")

        pos_x = round((random.random() * pos_range * 2) - pos_range, 1)
        pos_y = round((random.random() * pos_range * 2) - pos_range, 1)
        pos_z = 0.0

        rot_x = 0.0
        rot_y = 0.0
        rot_z = round(random.random() * 360.0, 1)

        fileText += '\t{\n\t"Name":"'+ prefabNames[index]+'",\n'

        fileText += '\t\t"position": {\n'
        fileText += '\t\t\t"x": '+str(pos_x)+',\n'
        fileText += '\t\t\t"y": '+str(pos_y)+',\n'
        fileText += '\t\t\t"z": '+str(pos_z)+'\n'
        fileText += '\t\t},\n\n'

        fileText += '\t\t"rotation": {\n'
        fileText += '\t\t\t"x": '+str(rot_x)+',\n'
        fileText += '\t\t\t"y": '+str(rot_y)+',\n'
        fileText += '\t\t\t"z": '+str(rot_z)+'\n'
        fileText += '\t\t},\n\n'

        fileText += '\t\t"scale": {\n'
        fileText += '\t\t\t"x": 1.0,\n'
        fileText += '\t\t\t"y": 1.0,\n'
        fileText += '\t\t\t"z": 1.0\n'
        fileText += '\t\t}\n\n'

        fileText += '\t}'+("," if i < numPrefabs-1 else "")+'\n'

    fileText += '\n]'

    # Writing to .scene file

    print("Writing to file...")

    out_file = open(output_path, mode="w")
    out_file.write(fileText)
    out_file.close()

    print("Generateing Resources...")

    subprocess.call(['C:/dev/csharp/nephilim/Nephilim.Desktop/GenResources.bat'])

    print("Complete!")

    


if __name__ == "__main__":
    main()