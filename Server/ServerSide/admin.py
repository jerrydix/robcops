from django.contrib import admin

from .models import Player
from .models import FriendRequest

admin.site.register(Player)
admin.site.register(FriendRequest)
