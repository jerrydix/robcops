"""
URL configuration for Server project.

The `urlpatterns` list routes URLs to views. For more information please see:
    https://docs.djangoproject.com/en/4.2/topics/http/urls/
Examples:
Function views
    1. Add an import:  from my_app import views
    2. Add a URL to urlpatterns:  path('', views.home, name='home')
Class-based views
    1. Add an import:  from other_app.views import Home
    2. Add a URL to urlpatterns:  path('', Home.as_view(), name='home')
Including another URLconf
    1. Import the include() function: from django.urls import include, path
    2. Add a URL to urlpatterns:  path('blog/', include('blog.urls'))
"""
from django.contrib import admin
from django.urls import path, include

from ServerSide import views

# to send and accept requests - just write the following addresses in client


urlpatterns = [
    path('admin/', admin.site.urls),
    path('send_friend_request/<int:userID>/', views.send_friend_request, name='send_friend_request'),
    path('accept_friend_request/<int:requestID>/', views.accept_friend_request, name='accept_friend_request'),

    path('edit_money/', views.edit_money, name='edit_money'),
    path('get_money/', views.get_money, name='get_money'),
    path('switch_to_robber/', views.switch_to_robber, name='switch_to_robber'),
    path('switch_to_cop/', views.switch_to_cop, name='switch_to_cop'),

    # Lobby Settings
    path('create_personal_safe/', views.create_personal_safe, name='create_personal_safe'),
    path('create_safe/', views.create_safe, name="create_safe"),
    path('create_lobby/<int:safeID>/', views.create_lobby, name='create_lobby'),
    path('get_lobby_members/', views.get_lobby_members, name='get_lobby_members'),
    path('damage_safe/', views.damage_safe, name='damage_safe'),
    path('getTimeUntilEnd/', views.getTimeUntilEnd, name='getTimeUntilEnd'),
    path('joinToEvent/<int:safeId>/', views.joinToEvent, name='joinToEvent'),
    path('checkIfStarted/', views.checkIfStarted, name='checkIfStarted'),
    path('startBreakIn/', views.startBreakIn, name='startBreakIn'),
    path('checkLobby/<int:safeId>/', views.checkLobby, name='checkLobby'),
    path('check_lobby_info/', views.check_lobby_info, name='check_lobby_info'),
    path('destroy_event/', views.destroy_event, name='destroy_event'),
    path('leave_lobby/', views.leave_lobby, name='leave_lobby'),
    path('start_robbery/', views.start_robbery, name='start_robbery'),
    path('get_safe_hp/', views.get_safe_hp, name='get_safe_hp'),
    path('check_if_arrested/', views.check_if_arrested, name='check_if_arrested'),
    path('end_robbery_success/', views.end_robbery_success, name='end_robbery_success'),
    path('end_robbery_unsuccess/', views.end_robbery_unsuccess, name='end_robbery_unsuccess'),

    # Get Lists
    path('get_all_safes/', views.get_all_safes, name='get_all_safes'),
    path('get_all_robunions/', views.get_all_robunions, name='get_all_robunions'),
    path('get_robunion_members/<int:robId>', views.get_robunion_members, name='get_robunion_members'),
    path('get_all_locations/', views.get_all_locations, name='get_all_locations'),

    # Guild
    path('create_test_robunion/', views.create_test_robunion, name='create_test_robunion'),
    path('create_police_station/', views.create_police_station, name='create_police_station'),
    path('join_rob_union/<int:robId>/', views.join_rob_union, name='join_rob_union'),
    path('join_police_station/<int:policeId>/', views.join_police_station, name='join_police_station'),
    path('buy_powerup/<int:item>/', views.buy_powerup, name='buy_powerup'),
    path('donate_to_guild/', views.donate_to_guild, name='donate_to_guild'),
    path('add_upgrades_to_lobby/<int:item>/', views.add_upgrades_to_lobby, name='add_upgrades_to_lobby'),
    path('get_machines/', views.get_machines, name='get_machines'),
    path('get_police_stats/', views.get_police_stats, name='get_police_stats'),
    path('get_guild_money/', views.get_guild_money, name='get_guild_money'),
    path('buy_new_machine/', views.buy_new_machine, name='buy_new_machine'),

    # Amount of clicks
    path('get_amount_of_clicks/', views.get_amount_of_clicks, name='get_amount_of_clicks'),
    path('upgrade_click_power/', views.upgrade_click_power, name="upgrade_click_power"),
    path('upgrade_amount_of_clicks/', views.upgrade_amount_of_clicks, name="upgrade_amount_of_clicks"),

    # path('members/', include('django.contrib.auth.urls'))
    path('members/login_user', views.login_user, name='login_user'),
    path('members/logout_user', views.logout_user, name='logout_user'),
    path('members/register_user', views.register_user, name='register_user'),
]
