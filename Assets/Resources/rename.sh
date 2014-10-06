echo "Renaming files..."
for dir in $(find ./ -type d); do
	rename 's/600-x-1024.png/.png/' *.png
done;
echo "Done."

