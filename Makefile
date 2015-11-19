build:
	xbuild Scripts/DapCore.sln
	cp -v Scripts/bin/Debug/Dap* Releases/Latest/
