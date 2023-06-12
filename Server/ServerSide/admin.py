from django.contrib import admin

from .models import Player
from .models import FriendRequest
from .models import Guild

admin.site.register(Player)
admin.site.register(FriendRequest)
admin.site.register(Guild)