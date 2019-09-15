FROM ubuntu:18.04

LABEL maintainer="gutee"

RUN apt-get update
RUN apt-get install -y python3 python3-dev python3-pip nginx unixodbc-dev curl libssl1.0.0 libssl-dev

COPY ./requirements.txt /app/requirements.txt

WORKDIR /app

#download odbc driver for linux container
RUN curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
RUN curl https://packages.microsoft.com/config/ubuntu/18.04/prod.list > /etc/apt/sources.list.d/mssql-release.list

#to install odbc driver
RUN apt-get -y update
RUN ACCEPT_EULA=Y apt-get -y install msodbcsql17
RUN ACCEPT_EULA=Y apt-get -y install mssql-tools
RUN echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bash_profile
RUN echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc

#RUN source ~/.bashrc
RUN apt-get -y install unixodbc-dev

RUN pip3 install -r requirements.txt

COPY . /app

EXPOSE 80

ENTRYPOINT ["/usr/bin/python3"]

CMD ["internet-app.py"]