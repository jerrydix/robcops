from channels.generic.websocket import WebsocketConsumer


class LobbyConsumer(WebsocketConsumer):
    def connect(self):
        self.accept()
        self.send("Connected")
       