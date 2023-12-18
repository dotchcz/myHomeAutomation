# Home Automation System README

## What does this project do?

This open-source project is designed to simplify the collection and storage of data from various sensors. It consists of three components: a Database (DB), a Front End (FE) using Angular, and a Back End (BE) developed as a WebApi. The primary function is to gather data through HTTP requests to the API and manage action elements like Wedos Mini D1 with a web server and Tasmota plug. These components are encapsulated in Docker images, optimized for the Raspberry Pi 4 ARM platform, and are deployable using docker-compose. Additionally, the system is compatible with Kubernetes (K8S), providing a scalable and robust deployment option, currently in an experimental stage.

## Why is this project useful?

This system offers a user-friendly solution for home automation, integrating temperature sensors and control elements for small appliances such as lights and circulation pumps. It aims to provide an accessible yet powerful tool for home automation enthusiasts, leveraging open-source flexibility.
The system can serve as a very simple bases for your own home automation. In my case I'm able to measure a temperature in my heating system and based on that I remotely control a circulation pump so it runs when there is an appropriate level of temperature only.  

## How do I get started?

- Initial Setup: Ensure you have a Raspberry Pi 4 with Docker and docker-compose installed.
- Clone the Repository: Obtain the project code from the repository.
- Configuration: Follow the setup instructions to configure the DB, FE, and BE components.
- Docker Deployment: Use docker-compose bash script called `deploy.sh` to deploy the Docker images on your Raspberry Pi.
- Kubernetes Deployment: For experimental K8S deployment, follow the additional Kubernetes-specific instructions.
- Running the System: Start the system and begin integrating your sensors and action elements. It's important to run all 
the sensors or other devices in the same local network. I highly recommend to use WeMos D1 Mini ESP8266 WiFi modules for sensors and smart plug wifi Tasmota for action units.

## K8S support 

The system is ready to run in K8S as well. This part is experimentaly proven, it's now in the main development process.

## Contributing guidelines

- In case of any issue, please, create a pull request with relevant fix, create an issue if needed or contact me via email.
- In case of suggesting a new feature, please, contact me via email and create relevant pull request.
- Please, feel free even to suggest any improvements and we all can teach from each other ;) 

## Where can I get more help, if I need it?

For more information or assistance, please reach out to us at ja.jan.ja@seznam.cz. We are here to help with any questions or issues you may encounter while setting up or using the system.