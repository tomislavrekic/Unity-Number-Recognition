import random
import sys
import io
from pathlib import Path
import math


#w=np.random.randn(layer_size[l],layer_size[l-1])*np.sqrt(2/layer_size[l-1])


inputSize = 784
firstLayerSize = 16
secondLayerSize = 16
outputLayerSize = 10

up = 2
down = -2


checkIfExists = Path("data.txt")
if checkIfExists.is_file() :
    print("File Exists, remove it first.")
    exit()
    
    
dataFile = open("data.txt", "w")

for i in range (0,firstLayerSize):
    dataFile.write( str(random.uniform(down, up)) + ' ' )

for i in range (0,secondLayerSize):
    dataFile.write( str( random.uniform(down, up)) + ' ' )

for i in range (0,outputLayerSize):
    dataFile.write( str( random.uniform(down, up) ) + ' ' )

for i in range (0,inputSize):    
    for j in range (0,firstLayerSize):
        dataFile.write( str( random.uniform(down, up)) + ' ' )

for i in range (0,firstLayerSize):    
    for j in range (0,secondLayerSize):
        dataFile.write( str( random.uniform(down, up) ) + ' ' )

for i in range (0,secondLayerSize):    
    for j in range (0,outputLayerSize):
        dataFile.write( str( random.uniform(down, up)) + ' ' )


'''

for i in range (0,firstLayerSize):
    dataFile.write( str( random.uniform(firstLayerSize, inputSize) * math.sqrt(2/inputSize) ) + ' ' )

for i in range (0,secondLayerSize):
    dataFile.write( str( random.uniform(secondLayerSize, firstLayerSize) * math.sqrt(2/firstLayerSize) ) + ' ' )

for i in range (0,outputLayerSize):
    dataFile.write( str( random.uniform(outputLayerSize, secondLayerSize) * math.sqrt(2/secondLayerSize) ) + ' ' )

for i in range (0,inputSize):    
    for j in range (0,firstLayerSize):
        dataFile.write( str( random.uniform(firstLayerSize, inputSize) * math.sqrt(2/inputSize) ) + ' ' )

for i in range (0,firstLayerSize):    
    for j in range (0,secondLayerSize):
        dataFile.write( str( random.uniform(secondLayerSize, firstLayerSize) * math.sqrt(2/firstLayerSize) ) + ' ' )

for i in range (0,secondLayerSize):    
    for j in range (0,outputLayerSize):
        dataFile.write( str( random.uniform(outputLayerSize, secondLayerSize) * math.sqrt(2/secondLayerSize) ) + ' ' )

'''