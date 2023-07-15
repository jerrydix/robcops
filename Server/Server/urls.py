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
    path('switch_role/', views.switch_role, name='switch_role'),
    path('pay_money/', views.pay_money, name='pay_money'),
    path('start_safe_farm/', views.start_safe_farm, name='start_safe_farm'),
    path('get_robberxp/', views.get_robberxp, name='get_robberxp'),
    path('get_policexp/', views.get_policexp, name='get_policexp'),
    path('get_safe_status/', views.get_safe_status, name='get_safe_status'),
    path('leave_guild/', views.leave_guild, name='leave_guild'),
    path('get_robberxp/', views.get_robberxp, name='get_robberxp'),
    path('get_policexp/', views.get_policexp, name='get_policexp'),
    path('edit_policexp/', views.edit_policexp, name='edit_policexp'),
    path('edit_robberxp/', views.edit_robberxp, name='edit_robberxp'),
    path('place_safe/', views.place_safe, name='place_safe'),
    path('generate_robunion/', views.generate_robunion, name='generate_robunion'),
    path('send_location/', views.send_location, name='send_location'),
    path('generate_random_safe/', views.generate_random_safe, name='generate_random_safe'),
    path('are_safes_near_you/', views.are_safes_near_you, name='are_safes_near_you'),
    path('give_hint/', views.give_hint, name='give_hint'),
    path('give_hint_test/<int:copId>/', views.give_hint_test, name='give_hint_test'),
    path('check_safes/', views.check_safes, name='check_safes'),

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
    path('safe_is_robunion/<int:safeId>/', views.safe_is_robunion, name='safe_is_robunion'),
    path('check_lobby_info/', views.check_lobby_info, name='check_lobby_info'),
    path('destroy_event/', views.destroy_event, name='destroy_event'),
    path('leave_lobby/', views.leave_lobby, name='leave_lobby'),
    path('start_robbery/', views.start_robbery, name='start_robbery'),
    path('get_safe_hp/', views.get_safe_hp, name='get_safe_hp'),
    path('check_if_arrested/', views.check_if_arrested, name='check_if_arrested'),
    path('end_robbery_success/', views.end_robbery_success, name='end_robbery_success'),
    path('end_robbery_unsuccess/', views.end_robbery_unsuccess, name='end_robbery_unsuccess'),
    path('end_robbery_unsuccess_without_penalty/', views.end_robbery_unsuccess_without_penalty,
         name='end_robbery_unsuccess_without_penalty'),
    path('arrest_lobby/', views.arrest_lobby, name='arrest_lobby'),
    path('get_arrest_status/', views.get_arrest_status, name='get_arrest_status'),
    path('damage_safe_memory/', views.damage_safe_memory, name='damage_safe_memory'),
    path('damage_safe_maze/', views.damage_safe_maze, name='damage_safe_maze'),
    path('joinToRaid/<int:safeId>/', views.joinToRaid, name='joinToRaid'),

    # Get Lists
    path('get_all_safes/', views.get_all_safes, name='get_all_safes'),
    path('get_all_robunions/', views.get_all_robunions, name='get_all_robunions'),
    path('get_all_stations/', views.get_all_stations, name='get_all_stations'),
    path('get_robunion_members/', views.get_robunion_members, name='get_robunion_members'),
    path('get_station_members/', views.get_station_members, name='get_station_members'),
    path('get_robunion_info/', views.get_robunion_info, name='get_robunion_info'),
    path('get_station_info/', views.get_station_info, name='get_station_info'),
    path('get_all_locations/', views.get_all_locations, name='get_all_locations'),
    path('get_player_info/', views.get_player_info, name='get_player_info'),

    # Guild
    path('create_robunion/', views.create_robunion, name='create_robunion'),
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
    path('get_robunion_info/', views.get_robunion_info, name='get_robunion_info'),
    path('start_machine_farm/', views.start_machine_farm, name='start_machine_farm'),
    path('get_robunion_coordinates/', views.get_robunion_coordinates, name='get_robunion_coordinates'),
    path('leave_guild/', views.leave_guild, name='leave_guild'),
    path('get_robunion_safe_id/', views.get_robunion_safe_id, name='get_robunion_safe_id'),
    path('reset_robunion_safe_id/', views.reset_robunion_safe_id, name='reset_robunion_safe_id'),

    # Amount of clicks
    path('get_amount_of_clicks/', views.get_amount_of_clicks, name='get_amount_of_clicks'),
    path('get_click_power/', views.get_click_power, name='get_click_power'),

    path('upgrade_click_power/', views.upgrade_click_power, name="upgrade_click_power"),
    path('upgrade_amount_of_clicks/', views.upgrade_amount_of_clicks, name="upgrade_amount_of_clicks"),
    path('upgrade_armor/', views.upgrade_armor, name="upgrade_armor"),
    path('upgrade_weapons/', views.upgrade_weapons, name="upgrade_weapons"),
    path('update_hints/', views.update_hints, name="update_hints"),

    # path('members/', include('django.contrib.auth.urls'))
    path('members/login_user', views.login_user, name='login_user'),
    path('members/logout_user', views.logout_user, name='logout_user'),
    path('members/register_user', views.register_user, name='register_user'),
]
