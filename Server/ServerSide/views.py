from django.contrib.auth.decorators import login_required
from django.contrib.auth import authenticate, login, logout
from django.contrib.auth.forms import UserCreationForm
from django.contrib import messages
from django.http import HttpResponse

from ServerSide.models import FriendRequest
from ServerSide.models import Player

from django.shortcuts import render

# User auth

def login_user(request):
    if request.method == "POST":
        username = request.POST["username"]
        password = request.POST["password"]
        user = authenticate(request, username=username, password=password)
        if user is not None:
            login(request, user)
            return HttpResponse('Login Success')
        else:
            return HttpResponse('Login failure')

    else: 
        return HttpResponse('Login failure')
    
@login_required
def logout_user(request):
    logout(request)
    return HttpResponse('Logout Success')

def register_user(request):
    if request.method == "POST":
        form = UserCreationForm(request.POST)
        if form.is_valid():
            form.save()
            username = form.cleaned_data['username']
            password = form.cleaned_data['password1']
            user = authenticate(username=username, password=password)
            login(request, user)
            return HttpResponse('Register Success')
        
    else: 
        form = UserCreationForm()
    return HttpResponse('Register Success')


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
    response = f'0: money player: {request.user.player.money}'
    return HttpResponse(response)


def get_amount_of_clicks(request):
    if not request.user.is_authenticated:
        return HttpResponse('Not signed in')
    if request.method != 'GET':
        return HttpResponse('Incorrect request method')
    response = f'amount player: {request.user.player.amountOfClicks}'
    return HttpResponse(response)


def edit_amount_of_clicks(request):
    if not request.user.is_authenticated:
        return HttpResponse('Not signed in')
    if request.method != 'POST':
        return HttpResponse('Incorrect request method')
    amountOfClicks = request.POST['amountOfClicks']
    request.user.player.amountOfClicks = int(amountOfClicks)
    request.user.player.save()
    response = f'0: changed the amountOfClicks of {request.user.username} to {amountOfClicks}'
    return HttpResponse(response)

