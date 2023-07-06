import datetime
import math
import random
import threading
import time

from django.contrib.auth.decorators import login_required
from django.contrib.auth import authenticate, login, logout
from django.contrib.auth.forms import UserCreationForm
from django.contrib import messages
from django.http import HttpResponse, StreamingHttpResponse

from ServerSide.models import FriendRequest
from ServerSide.models import *
from django.db import transaction

from django.shortcuts import render


# User auth


def login_user(request):
    if request.method == "POST":
        username = request.POST["username"]
        password = request.POST["password"]
        user = authenticate(request, username=username, password=password)
        if user is not None:
            login(request, user)
            return HttpResponse(
                f'1|{request.user.username}|{request.user.player.money}|{request.user.player.amountOfClicks}|'
                f'{request.user.player.clickPower}|{request.user.player.locationX}|{request.user.player.locationY}|'
                f'{request.user.player.role}|{request.user.player.robUnion_id}|'
                f'{request.user.player.policeStation_id}|{request.user.player.friends}')
        else:
            return HttpResponse('0|User not found / Wrong password')  # TODO wrong password abfrage (this)

    else:
        return HttpResponse('0|Request failed')


@login_required
def logout_user(request):
    logout(request)
    return HttpResponse('Logout Success')


@login_required
def get_player_info(request):
    return HttpResponse(
        f'1|{request.user.username}|{request.user.player.money}|{request.user.player.amountOfClicks}|'
        f'{request.user.player.clickPower}|{request.user.player.locationX}|{request.user.player.locationY}|'
        f'{request.user.player.role}|{request.user.player.robUnion_id}|'
        f'{request.user.player.policeStation_id}|{request.user.player.friends}')


def register_user(request):
    if request.method == "POST":
        form = UserCreationForm(request.POST)
        error_message = form.errors.as_text()
        if form.is_valid():
            form.save()
            username = form.cleaned_data['username']
            password = form.cleaned_data['password1']
            user = authenticate(request, username=username, password=password)
            login(request, user)
            player = Player(user=user)
            player.save()
            return HttpResponse('1|Signup Successful')
        else:
            return HttpResponse(f'0|Register Form is not valid:{error_message}')
    return HttpResponse('0|Register failed')


@login_required
def send_friend_request(request, userID):
    from_user = request.user
    to_user = Player.user.objects.get(id=userID)
    friend_request, created = FriendRequest.objects.get_or_create(from_user=from_user, to_user=to_user)
    if created:
        return HttpResponse('Friend request is successfully sent')
    else:
        return HttpResponse('Friend Request Duplicate')


@login_required
def accept_friend_request(request, requestID):
    friend_request = FriendRequest.objects.get(id=requestID)
    if friend_request.to_user == request.user:
        friend_request.to_user.friends.add(friend_request.from_user)
        friend_request.from_user.friends.add(friend_request.to_user)
        friend_request.delete()
        return HttpResponse('Accepted')
    else:
        return HttpResponse('Not accepted')


@login_required
def edit_money(request):
    if not request.user.is_authenticated:
        return HttpResponse('Not signed in')
    if request.method != 'POST':
        return HttpResponse('Incorrect request method')
    money = request.POST['money']
    request.user.player.money = int(money)
    request.user.player.save()
    response = f'0: changed the money of {request.user.username} to {money}'
    return HttpResponse(response)


def get_money(request):
    if not request.user.is_authenticated:
        return HttpResponse('Not signed in')
    if request.method != 'GET':
        return HttpResponse('Incorrect request method')
    return HttpResponse(request.user.player.money)


def get_amount_of_clicks(request):
    if not request.user.is_authenticated:
        return HttpResponse('Not signed in')
    if request.method != 'GET':
        return HttpResponse('Incorrect request method')
    response = f'amount player: {request.user.player.amountOfClicks}'
    return HttpResponse(response)


@login_required
def create_personal_safe(request):
    safe = Safe(locationX=request.user.player.locationX, locationY=request.user.player.locationY)
    safe.save()
    return HttpResponse(safe.id)


@login_required
def create_safe(request):
    if request.method != 'POST':
        return HttpResponse('Incorrect request method')
    else:
        lvl = request.POST["level"]
        hp = request.POST["hp"]
        x = request.POST["locationX"]
        y = request.POST["locationY"]
        safe = Safe(level=lvl, hp=hp, locationX=float(x), locationY=float(y))
        safe.save()
        response = f"Safe {safe.id} level {lvl} with {hp} hp placed at the following location: {x}, {y}"
        return HttpResponse(response)


@login_required
def pay_money(request):
    if request.method != 'POST':
        return HttpResponse('Incorrect request method')
    else:
        cost = request.POST["cost"]
        request.user.player.money -= int(cost)
        request.user.player.save()
        return HttpResponse(request.user.player.money)


@login_required
def create_test_robunion(request):
    if not request.user.player.role:
        robunion = RobUnion(name=request.POST['name'])
        robunion.save()
        request.user.player.robUnion = robunion
        request.user.player.policeStation = None
        request.user.player.save()
    else:
        return HttpResponse("You are policeman")
    return HttpResponse(robunion.id)


@login_required
def create_police_station(request):
    if not request.user.player.role:
        police_station = PoliceStation(name=request.POST['name'])
        police_station.save()
        request.user.player.policeStation = police_station
        request.user.player.robUnion = None
        request.user.player.save()
    else:
        return HttpResponse("You are robber")
    return HttpResponse(police_station.id)


@login_required
def create_lobby(request, safeID):
    safe = Safe.objects.get(id=safeID)
    breakIn = BreakInEvent(safe=safe)
    breakIn.save()
    request.user.player.event = breakIn
    request.user.player.save()
    return HttpResponse(request.user.username)


def get_all_safes(request):
    response = "|".join(
        str(e).replace("(", "").replace(")", "").replace(" ", "") for e in list(Safe.objects.values_list(
            'id',
            'level',
            'hp',
            'locationX',
            'locationY')))
    return HttpResponse(response)


def get_all_robunions(request):
    response = "|".join(str(e).replace("(", "").replace(")", "").replace("\'", "") for e in list(
        RobUnion.objects.values_list('id',
                                     'name')))
    return HttpResponse(response)


def get_all_stations(request):
    response = "|".join(
        str(e).replace("(", "").replace(")", "").replace("\'", "") for e in list(PoliceStation.objects.values_list('id',
                                                                                                                   'name')))
    return HttpResponse(response)


def get_station_members(request):
    response = "|".join(str(e) for e in list(PoliceStation.objects.get(
        id=request.user.player.policeStation.id).police_station.all()))
    return HttpResponse(response)


@login_required
def get_station_info(request):
    response = f"{request.user.player.policeStation.id}|{request.user.player.policeStation.name}|" \
               f"{request.user.player.policeStation.weaponLvl}|{request.user.player.policeStation.armorLvl}|" \
               f"{request.user.player.policeStation.hints}"
    return HttpResponse(response)


def get_robunion_members(request):
    response = "|".join(str(e) for e in list(RobUnion.objects.get(
        id=request.user.player.robUnion.id).robUnion.all()))
    return HttpResponse(response)


@login_required
def upgrade_weapons(request):
    if request.method != 'POST':
        return HttpResponse('Incorrect request method')
    else:
        cost = request.POST["cost"]
        if request.user.player.policeStation.guildMoney < int(cost):
            return HttpResponse("0|Not Enough Money")
        else:
            request.user.player.policeStation.guildMoney -= int(cost)
            request.user.player.policeStation.weaponLvl += 1
            request.user.player.policeStation.save()
    return HttpResponse(request.user.player.policeStation.guildMoney)


@login_required
def upgrade_armor(request):
    if request.method != 'POST':
        return HttpResponse('Incorrect request method')
    else:
        cost = request.POST["cost"]
        if request.user.player.policeStation.guildMoney < int(cost):
            return HttpResponse("0|Not Enough Money")
        else:
            request.user.player.policeStation.guildMoney -= int(cost)
            request.user.player.policeStation.armorLvl += 1
            request.user.player.policeStation.save()
    return HttpResponse(request.user.player.policeStation.guildMoney)


@login_required
def update_hints(request):
    request.user.player.policeStation.hints += 1
    return HttpResponse(request.user.player.policeStation.hints)


@login_required
def get_robunion_info(request):
    response = f"{request.user.player.robUnion.id}|{request.user.player.robUnion.name}"
    return HttpResponse(response)


@login_required
def get_lobby_members(request):
    response = request.user.player.event.members.count()
    return HttpResponse(response)


@transaction.atomic()
@login_required
def damage_safe(request):
    damage = int(request.user.player.amountOfClicks * request.user.player.clickPower)
    safe = Safe.objects.select_for_update().get(id=request.user.player.event.safe.id)
    safe.hp -= damage
    safe.save()
    return HttpResponse(safe.hp)


@login_required
def getTimeUntilEnd(request):
    now = datetime.datetime.now()
    start = request.user.player.event.startTime.replace(tzinfo=None)
    diff = now - start
    return HttpResponse(diff)


@login_required
def checkLobby(request, safeId):
    safe = Safe.objects.get(id=safeId)
    if hasattr(safe, "breakinevent"):
        return HttpResponse("0")
    else:
        return HttpResponse("1")


@login_required
def joinToEvent(request, safeId):
    safe = Safe.objects.get(id=safeId)
    if not safe.breakinevent.isStarted or safe.breakinevent.members.count() < 5:
        request.user.player.event = safe.breakinevent
        request.user.player.save()
        response = f"1|{request.user.player.event.members.count()}|"
        for member in request.user.player.event.members.all():
            response += member.user.username + "|"
        return HttpResponse(response)
    else:
        return HttpResponse("0|You can't join the event")


@login_required
def checkIfStarted(request):
    return HttpResponse(request.user.player.event.isStarted)


@login_required
def startBreakIn(request):
    request.user.player.event.isStarted = True
    request.user.player.event.startTime = datetime.datetime.now()
    request.user.player.event.save()
    return HttpResponse(request.user.player.event.startTime)


def get_all_locations(request):
    response = "|".join(str(e).replace("(", "").replace(")", "") for e in list(Player.objects.values_list('role',
                                                                                                          'locationX',
                                                                                                          'locationY')))
    return HttpResponse(response)


@login_required
def check_lobby_info(request):
    response = str(request.user.player.event.members.count()) + "|" + str(request.user.player.event.isStarted) + "|"
    response += str(request.user.player.event.c4s) + "|"
    response += str(request.user.player.event.alarms) + "|"
    for member in request.user.player.event.members.all():
        response += member.user.username + "|"
    return HttpResponse(response)


@login_required
def buy_powerup(request, item):
    union = request.user.player.robUnion
    members = union.robUnion.all()
    if item == 0:
        if union.guildMoney < 1000:
            return HttpResponse("0|Not enough money")
        else:
            union.guildMoney -= 1000
            for a in members:
                a.c4 += 1
                a.save()
            union.save()
            return HttpResponse(union.guildMoney)
    elif item == 1:
        if union.guildMoney < 5000:
            return HttpResponse("0|Not enough money")
        else:
            union.guildMoney -= 5000
            for a in members:
                a.alarmDisabler += 1
                a.save()
            union.save()
            return HttpResponse(union.guildMoney)
    return HttpResponse(union.guildMoney)


@login_required
def join_rob_union(request, robId):
    if request.user.player.role == 1:
        return HttpResponse("0| You are cop. Have a good day")
    else:
        request.user.player.policeStation = None
        union = RobUnion.objects.get(id=robId)
        request.user.player.robUnion = union
        request.user.player.save()
        return HttpResponse(request.user.player.robUnion.id)


@login_required
def join_police_station(request, policeId):
    if request.user.player.role == 0:
        return HttpResponse("0| You are rob. Have a good day")
    else:
        request.user.player.robUnion = None
        station = PoliceStation.objects.get(id=policeId)
        request.user.player.policeStation = station
        request.user.player.save()
        return HttpResponse(request.user.player.policeStation.id)


@login_required
def donate_to_guild(request):
    money = int(math.floor(request.user.player.money / 2))
    if request.user.player.role == 0:
        request.user.player.robUnion.guildMoney += money
        request.user.player.money -= money
        request.user.player.robUnion.save()
        request.user.player.save()
        response = f"{request.user.player.robUnion.guildMoney}"
        return HttpResponse(response)
    else:
        request.user.player.policeStation.guildMoney += money
        request.user.player.money -= money
        request.user.player.policeStation.save()
        request.user.player.save()
        response = f"{request.user.player.policeStation.guildMoney}"
        return HttpResponse(response)


@login_required
def upgrade_amount_of_clicks(request):
    if request.method != 'POST':
        return HttpResponse("0|False method")
    else:
        cost = request.POST["cost"]
        if request.user.player.money < cost:
            return HttpResponse("0|You have not enough money")
        else:
            request.user.player.money -= cost
            request.user.player.amountOfClicks += 1
            request.user.player.save()
            response = f"{request.user.player.amountOfClicks}"
            return HttpResponse(response)


@login_required
def upgrade_click_power(request):
    if request.method != 'POST':
        return HttpResponse("0|False method")
    else:
        cost = request.POST["cost"]
        if request.user.player.money < cost:
            return HttpResponse("0|You have not enough money")
        else:
            request.user.player.money -= cost
            request.user.player.clickPower += 1
            request.user.player.save()
            response = f"{request.user.player.clickPower}"
            return HttpResponse(response)


@login_required
def add_upgrades_to_lobby(request, item):
    if item == 0:
        if request.user.player.c4 < 1:
            return HttpResponse("0|You have not enough c4s")
        else:
            request.user.player.event.c4s += 1
            request.user.player.c4 -= 1
            request.user.player.save()
            request.user.player.event.save()
            return HttpResponse(request.user.player.event.c4s)
    elif item == 1:
        if request.user.player.alarmDisabler < 1:
            return HttpResponse("0|You have not enough alarms")
        else:
            request.user.player.event.alarms += 1
            request.user.player.alarmDisabler -= 1
            request.user.player.event.save()
            request.user.player.save()
            return HttpResponse(request.user.player.event.alarms)


@login_required
def get_guild_money(request):
    if request.user.player.role == 0:
        return HttpResponse(request.user.player.robUnion.guildMoney)
    else:
        return HttpResponse(request.user.player.policeStation.guildMoney)


@login_required
def get_machines(request):
    return HttpResponse(request.user.player.robUnion.machines)


@login_required
def get_police_stats(request):
    response = f'{request.user.player.policeStation.weaponLvl}|{request.user.player.policeStation.armorLvl}'
    return HttpResponse(response)


@login_required
def leave_lobby(request):
    request.user.player.event = None
    request.user.player.save()
    return HttpResponse(request.user.player.event)


@login_required
def destroy_event(request):
    request.user.player.event.delete()
    return HttpResponse(request.user.player.event)


@login_required
def get_safe_hp(request):
    return HttpResponse(request.user.player.event.safe.hp)


# TODO Balance
@login_required
def start_robbery(request):
    breakInCurrent = request.user.player.event
    breakInCurrent.isStarted = True
    breakInCurrent.startTime = datetime.datetime.now()
    breakInCurrent.safe.hp -= breakInCurrent.c4s * 500
    breakInCurrent.safe.status = 2
    breakInCurrent.safe.save()
    breakInCurrent.timeForRobbery += breakInCurrent.alarms * 0.16
    breakInCurrent.timeForRobbery = 1.0
    breakInCurrent.reward = breakInCurrent.safe.level * random.randint(10000, 30000)
    breakInCurrent.save()
    response = f'{breakInCurrent.timeForRobbery}|{breakInCurrent.safe.hp}|{breakInCurrent.safe.level}'
    return HttpResponse(response)


@login_required
def check_if_arrested(request):
    return HttpResponse(request.user.player.event.arrested)


@login_required
def end_robbery_success(request):
    reward = request.user.player.event.reward
    request.user.player.money += request.user.player.event.reward
    request.user.player.save()
    if request.user.player.event is not None:
        if request.user.player.event.safe is not None:
            request.user.player.event.safe.delete()
            request.user.player.event.delete()
    return HttpResponse(f'{request.user.player.money}|{reward}')


@login_required
def end_robbery_unsuccess(request):
    old_money = request.user.player.money
    request.user.player.money /= 2
    request.user.player.save()
    if request.user.player.event is not None:
        if request.user.player.event.safe is not None:
            request.user.player.event.safe.delete()
            request.user.player.event.delete()
    return HttpResponse(f'{request.user.player.money}|{old_money}')


@login_required
def buy_new_machine(request):
    if request.method != 'POST':
        return HttpResponse("0|False method")
    else:
        if request.user.player.robUnion.machines >= 6:
            return HttpResponse(request.user.player.robUnion.machines)
        cost = request.POST["cost"]
        if request.user.player.robUnion.guildMoney < int(cost):
            return HttpResponse(0)
        else:
            request.user.player.robUnion.guildMoney -= int(cost)
            request.user.player.robUnion.save()
            request.user.player.robUnion.machines += 1
            request.user.player.robUnion.save()
    return HttpResponse(request.user.player.robUnion.machines)


def switch_role(request):
    if request.user.player.role is True:
        request.user.player.role = False
        request.user.player.policeStation = None
        request.user.player.robberXP = 0
        request.user.player.safesActive = 0
    else:
        request.user.player.role = True
        request.user.player.robUnion = None
        request.user.player.event = None
        request.user.player.c4 = 0
        request.user.player.alarmDisabler = 0
        request.user.player.policeXP = 0
    request.user.player.save()
    return HttpResponse(request.user.player.role)


@login_required
def send_location(request):
    if request.method != "POST":
        return HttpResponse("False response")
    else:
        x = request.POST["locationX"]
        y = request.POST["locationY"]
        request.user.player.locationX = x
        request.user.player.locationY = y
        request.user.player.save()
        return HttpResponse("1")


@login_required
def get_robberxp(request):
    return HttpResponse(request.user.player.robberXP)


@login_required
def get_policexp(request):
    return HttpResponse(request.user.player.policeXP)


def start_machine_farm(request):
    t = threading.Thread(target=update_money, args=[request])
    t.setDaemon(True)
    t.start()
    return HttpResponse("started")


def start_safe_farm(request):
    t = threading.Thread(target=update_policeman_money, args=[request])
    t.setDaemon(True)
    t.start()
    return HttpResponse("started")


def update_money(request):
    if request.user.player.robUnion is not None:
        ru = request.user.player.robUnion
        while True:
            time.sleep(3600)
            ru.guildMoney += ru.machines * 100000
            ru.save()


def update_policeman_money(request):
    policeman = request.user.player
    while True:
        time.sleep(3600)
        policeman.money += policeman.safesActive * 1000
        policeman.save()
