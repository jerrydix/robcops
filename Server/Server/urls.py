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

    path('edit_amount_of_clicks/', views.edit_amount_of_clicks, name='edit_amount_of_clicks'),
    path('get_amount_of_clicks/', views.get_amount_of_clicks, name='get_amount_of_clicks'),

    #path('members/', include('django.contrib.auth.urls'))
    path('members/login_user', views.login_user, name='login_user'),
    path('members/logout_user', views.logout_user, name='logout_user'),
    path('members/register_user', views.register_user, name='register_user'),
]
