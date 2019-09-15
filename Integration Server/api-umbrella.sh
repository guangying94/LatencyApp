#Get key for API Umbrella
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 367404D553B42995
echo "deb http://dl.bintray.com/nrel/api-umbrella-ubuntu bionic main" | sudo tee /etc/apt/sources.list.d/api-umbrella.list

#Install API Umbrella
sudo apt-get update
sudo apt-get -y install api-umbrella

#Start API Umbrella
sudo /etc/init.d/api-umbrella start