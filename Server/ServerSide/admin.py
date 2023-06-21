from django.contrib import admin

from .models import Player
from .models import FriendRequest
from .models import RobUnion
from .models import PoliceStation
from .models import Safe
from .models import BreakInEvent

admin.site.register(Player)
admin.site.register(FriendRequest)
admin.site.register(RobUnion)
admin.site.register(PoliceStation)
admin.site.register(Safe)
admin.site.register(BreakInEvent)
