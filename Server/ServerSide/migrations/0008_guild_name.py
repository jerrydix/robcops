# Generated by Django 4.2.1 on 2023-06-13 17:05

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('ServerSide', '0007_alter_player_friends'),
    ]

    operations = [
        migrations.AddField(
            model_name='guild',
            name='name',
            field=models.CharField(default='NoNameGuild', max_length=30),
        ),
    ]
