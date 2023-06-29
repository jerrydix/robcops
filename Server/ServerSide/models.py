import datetime

from django.contrib.auth.models import User
from django.db import models


# Current models: Player, Safe, Break_In_Event, Rob_Union, Police_Station


class Safe(models.Model):
    level = models.IntegerField(default=1)
    hp = models.IntegerField(default=100)
    locationX = models.FloatField(default=0.0)
    locationY = models.FloatField(default=0.0)


class BreakInEvent(models.Model):
    safe = models.OneToOneField(Safe, on_delete=models.CASCADE, primary_key=True)
    startTime = models.DateTimeField(default=datetime.datetime.now(), auto_now=False, auto_now_add=False)
    isStarted = models.BooleanField(default=False)


class Guild(models.Model):
    name = models.CharField(default="NoNameGuild", max_length=30)
    guildMoney = models.IntegerField(default=0)

    class Meta:
        abstract = True

    def __str__(self):
        return self.name


class PoliceStation(Guild):
    weaponLvl = models.IntegerField(default=1)


class RobUnion(Guild):
    machines = models.IntegerField(default=0)
    machinesLevels = models.IntegerField(default=1)
    hp = models.IntegerField(default=1)


class Player(models.Model):
    user = models.OneToOneField(User, on_delete=models.CASCADE, primary_key=True)
    friends = models.ManyToManyField("self", symmetrical=True, blank=True, default=None)
    money = models.IntegerField(default=0)
    amountOfClicks = models.IntegerField(default=1)
    clickPower = models.FloatField(default=1.0)
    locationX = models.FloatField(default=0.0)
    locationY = models.FloatField(default=0.0)
    role = models.BooleanField(default=0)
    robUnion = models.ForeignKey(RobUnion, null=True, blank=True, related_name='robUnion', on_delete=models.SET_NULL)
    policeStation = models.ForeignKey(PoliceStation, null=True, blank=True, related_name='police_station',
                                      on_delete=models.SET_NULL)
    event = models.ForeignKey(BreakInEvent, null=True, blank=True, related_name='members', on_delete=models.SET_NULL)
    policeXP = models.IntegerField(default=0)
    robberXP = models.IntegerField(default=0)

    def __str__(self):
        return self.user.username


# Extra Class to send a request to another User


class FriendRequest(models.Model):
    from_user = models.ForeignKey(Player, related_name='from_user', on_delete=models.CASCADE)
    to_user = models.ForeignKey(Player, related_name='to_user', on_delete=models.CASCADE)
