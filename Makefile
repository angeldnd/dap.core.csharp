version = 0.5.0

build:
	rm Releases/Latest/*
	xbuild Scripts/DapCore.sln
	rm Releases/$(version)/*
	cp Releases/Latest/Dap* Releases/$(version)/
	cd Releases/$(version) ; mmv -v "Dap*.dll" "Dap#1-$(version).dll"
	cd Releases/$(version) ; mmv -v "Dap*.dll.mdb" "Dap#1-$(version).dll.mdb"
	cd Releases/$(version) ; mmv -v "Dap*.xml" "Dap#1-$(version).xml"
	cd Releases/$(version) ; git checkout *.meta
	tools/git-local-summary > Releases/$(version)/DapCore-$(version).txt

