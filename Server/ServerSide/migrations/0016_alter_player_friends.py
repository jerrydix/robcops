# Generated by Django 4.2.1 on 2023-06-21 14:09

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('ServerSide', '0015_alter_player_friends'),
    ]

    operations = [
        migrations.AlterField(
            model_name='player',
            name='friends',
            field=models.ManyToManyField(to='ServerSide.player'),
        ),
    ]
