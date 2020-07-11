#!/bin/bash
set -ev
set echo off

pwd

echo NUGET_API_KEY ... contents hidden
echo NUGET_SOURCE == ${NUGET_SOURCE} 
echo TRAVIS_PULL_REQUEST == ${TRAVIS_PULL_REQUEST}
echo BUILD_CONFIG == ${BUILD_CONFIG}

if [ "${BUILD_CONFIG}" = "release" ]; then
		BUILD_DIR="Release"
else
		BUILD_DIR=$BUILD_CONFIG
fi
echo BUILD_DIR == ${BUILD_DIR}

dotnet build -c $BUILD_CONFIG ./ErrorCentral.sln

if [ $? -eq 0 ]
then
		if [ "$BUILD_CONFIG" = "not_happening" ]; 
		then
				dotnet test ./ErrorCentral.UnitTests/ErrorCentral.UnitTests.csproj -v normal --no-build
		else
				echo Skip testing on non-debug build
		fi
fi

if [ $? -eq 0 ]
then
		if [ "${TRAVIS_PULL_REQUEST}" = "false" ] && [ "${TRAVIS_BRANCH}" = "master" ]; 
		then
				dotnet nuget push ./ErrorCentral.API/bin/$BUILD_DIR/ErrorCentral.API.*.nupkg --api-key $NUGET_API_KEY --source $NUGET_SOURCE --skip-duplicate
		else
				echo Skip nuget for non-master build
		fi
fi
