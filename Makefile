build:
	xbuild Scripts/DapCore.sln
	cp -v Scripts/bin/Debug/* Releases/Latest/
