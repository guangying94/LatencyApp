echo "Installing Docker"

curl -fsSL https://get.docker.com -o get-docker.sh

sh get-docker.sh

echo "Add local users"

usermod -aG docker $USER

echo "Rebooting machine"

sudo reboot