# amazfitutils
Tools for working with Amazfit WatchFaces.

## Packing WatchFace
WatchFace.exe [-size] "path/to/config.json" [ "other/path/to/config.json" ... ]
WatchFace.exe -size176 bip.json
WatchFace.exe -size120x240 band.json
WatchFace.exe -size360 gtr.json
WatchFace.exe -size464 verge.json

## Unpacking WatchFace
WatchFace.exe [-size] "path/to/face.bin" [ "other/path/to/face.bin" ... ]
WatchFace.exe -size176 bip.bin
WatchFace.exe -size120x240 band.bin
WatchFace.exe -size360 gtr.bin
WatchFace.exe -size464 verge.bin
