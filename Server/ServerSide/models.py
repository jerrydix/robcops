from django.contrib.auth.models import User
from django.db import models

# Current models: Player, Safe, Break_In_Event, Rob_Union, Police_Station


class Player(models.Model):
    user = models.OneToOneField(User, on_delete=models.CASCADE, primary_key=True)
    friends = models.ManyToManyField("self", blank=True)
    money = models.IntegerField(default=0)
    amountOfClicks = models.IntegerField(default=1)
    clickPower = models.FloatField(default=1.0)

    def __str__(self):
        return self.user.username

# Extra Class to send a request to another User


class FriendRequest(models.Model):
    from_user = models.ForeignKey(Player, related_name='from_user', on_delete=models.CASCADE)
    to_user = models.ForeignKey(Player, related_name='to_user', on_delete=models.CASCADE)
