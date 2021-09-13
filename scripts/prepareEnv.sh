#!/bin/bash
# get aws credentials from circle ci context, and store it into file for use

set -euxo pipefail
awsDir=~/.aws
mkdir -p $awsDir
aws_access_key_id=${aws_access_key_id}
aws_secret_access_key=${aws_secret_access_key}

echo "Save AWS credential file"
cat > $awsDir/credentials << EOF
[default]
aws_access_key_id=$aws_access_key_id
aws_secret_access_key=$aws_secret_access_key
EOF

echo "Save AWS config file"
cat > $awsDir/config << EOF
[default]
region=ap-southeast-2
EOF
