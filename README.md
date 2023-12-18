# Home Automation System README

## What does this project do?

This open-source project focuses on simplifying the collection and storage of data from various sensors through HTTP requests to an API. It consists of three main components: a Database (DB), a Front End (FE) built with Angular, and a Back End (BE) developed as a WebApi. The Front End not only presents the measured data but also facilitates the control of action elements integrated into the entire setup. These action elements include devices like Wedos Mini D1 with a web server and Tasmota plug, both equipped with web servers, enabling control via HTTP requests. All three components are packaged into Docker images and deployed using docker-compose, specifically tailored for running on Raspberry Pi 4 ARM platforms.

## Why is this project useful?

This system is designed for straightforward home automation. It is equipped with temperature sensors and action elements to control small appliances such as lights and circulation pumps. This project aims to offer an accessible and user-friendly approach to home automation, leveraging the power of open-source for customization and flexibility.

## How do I get started?

Initial Setup: Ensure you have a Raspberry Pi 4 with Docker and docker-compose installed.
Clone the Repository: Obtain the project code from the repository.
Configuration: Follow the setup instructions to configure the DB, FE, and BE components.
Deployment: Use docker-compose bash script called `deploy.sh` to deploy the Docker images on your Raspberry Pi.
Running the System: Start the system and begin integrating your sensors and action elements. It's important to run all 
the sensors or other devices in the same local network. I highly recommend to use WeMos D1 Mini ESP8266 WiFi modules for sensors and smart plug wifi Tasmota for action units.

## Contributing guidelines

- In case of any issue, please, create a pull request with relevant fix, create an issue if needed or contact me via email.
- In case of suggesting a new feature, please, contact me via email and create relevant pull request.
- Please, feel free even to suggest any improvements and we all can teach from each other ;) 

## Where can I get more help, if I need it?

For more information or assistance, please reach out to us at ja.jan.ja@seznam.cz. We are here to help with any questions or issues you may encounter while setting up or using the system.