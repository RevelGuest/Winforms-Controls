all: mono

mono: swf-statusbar.cs
	mcs swf-statusbar.cs /r:System.Windows.Forms.dll

dotnet: swf-statusbar.cs
	csc swf-statusbar.cs /r:System.Windows.Forms.dll

clean:
	rm swf-statusbar.exe -r -f
