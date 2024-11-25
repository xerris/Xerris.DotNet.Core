#!/bin/bash

# Ensure the repository is fully fetched, including all tags
git fetch --tags

# Get all tags matching the format and sort them using version sort
all_tags=$(git tag -l "v*" | sort -V)

if [ -z "$all_tags" ]; then
    # If no matching tags exist, start with v0.0.1
    new_tag="v0.0.1"
else
    # Find the largest tag by sorting and getting the last one
    largest_tag=$(echo "$all_tags" | tail -n1)

    # Extract the version numbers
    version=$(echo $largest_tag | sed 's/v//')
    major=$(echo $version | cut -d. -f1)
    minor=$(echo $version | cut -d. -f2)
    patch=$(echo $version | cut -d. -f3)

    # Increment the patch number
    new_patch=$((patch + 1))

    # Construct the new tag
    new_tag="v$major.$minor.$new_patch"
fi

# Create and push the new tag
git tag $new_tag
git push origin $new_tag

echo "Created and pushed new tag: $new_tag"