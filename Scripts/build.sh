 #!/bin/bash
 
 TAG="test"

 while getopts t: flag
 do
	case "${flag}" in
		u)
			TAG=${OPTARG}
		;;
	esac
 done
 
 
echo "'$TAG' shall be used when building docker containers..."
  
# if an error occurs - exit out
set -e

# Regardless of where this script gets executed from
# we want to operate relative to this script location

SCRIPT_PATH=$(readlink -f "$0")
SCRIPT_DIR=$(dirname "$SCRIPT")

# BUILD BACKEND DOCKER FILE
echo "Building DevryDeveloperClub Backend..."
cd $SCRIPT_DIR/../DevryDeveloperClub
docker build -t devry-developer-club-backend:$TAG .

echo "Completed building DevryDeveloperClub Backend..."

cd $SCRIPT_DIR/../devry-developer-frontend

echo "Retrieving latest DevryDeveloperClub Frontend changes from main..."
git pull

echo "Building DevryDeveloperClub Frontend...
docker build -t devry-developer-club:$TAG .

