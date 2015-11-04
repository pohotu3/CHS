import os, sys

#[os.rename(f, f.replace(' ', '_')) for f in os.listdir(str(sys.argv[1])) if not f.startswith('.')]
for file in os.listdir(sys.argv[1]):
	os.renames(file, file.replace(' ', '_'))