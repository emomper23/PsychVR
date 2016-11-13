import fileinput
import sys
import glob


def replaceAll(file,searchExp,replaceExp):
    for line in fileinput.input(file, inplace=1):
        if searchExp in line:
            line = line.replace(searchExp,replaceExp)
        sys.stdout.write(line)

def main():
    print "starting..."
    files = glob.glob("*.meta")
    for f in files:
        replaceAll(f,"wrapMode: -1", "wrapMode: 1")

main()