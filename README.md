# JobCity Practical Exercise - StockChatApp

## Summary
The `StockChatApp` is a very simple browser chat application. It consists of a chat page where you can provide a username and start chatting. It also allows you to get stock quotes from a bot.

It has support for multiples users and provides access to the latest messages posted on the chat.

## How to run the application

With docker and docker-compose installed on your system, run the following command from the root folder of the project:

`docker-compose up -d --build`

- The command will take a few minutes to build.
- After the build is complete, you can access the chat on https://localhost:5076
- On the initial page, choose a username to access the chatroom
- On the chatroom, you can post the command to get the quote from the bot `e.g: /stock=aapl.us`

## Project Decisions

- I chose to use RabbitMQ to communicate between the API and the BOT
- I chose to use Redis to store the latest chat messages. I'm aware that this is not the best use case for Redis, but it was good enough for this application
- I prioritized building the happy path and consciously left out some input validations and exception handling.
- Following up the prioritization above, Only 2 test cases were implemented. I understand that in the real world you do not consider you app as "done" without enough tests to cover the main features, but I had to be pragmatic with the time I had.
- The app only supports one chatroom and there is no login in place.
