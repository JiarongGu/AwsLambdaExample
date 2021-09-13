#!/bin/bash
export PATH="$PATH:/root/.dotnet/tools"

apt-get update \
&& apt-get install zip -y \
&& apt-get install curl gnupg -yq \
&& curl -sL https://deb.nodesource.com/setup_14.x | bash \
&& apt-get install nodejs -yq

npm install -g serverless
npm install -g serverless-prune-plugin

dotnet tool install -g nbgv
dotnet tool install --tool-path . nbgv
dotnet tool install -g Amazon.Lambda.Tools

dotnet restore
nbgv prepare-release

# deploy lambda
set -a

cd src/AwsLambdaExample

if [ "$DEPLOYMENT_STAGE" != "prd" ]; then
    . ./Environments/$DEPLOYMENT_STAGE.env    
fi

dotnet lambda package --disable-interactive true --configuration Release --output-package ./bin/Release/netcoreapp3.1/AwsLambdaExample.zip
sls deploy --config ./serverless.yml