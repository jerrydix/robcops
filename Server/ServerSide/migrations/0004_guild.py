# Generated by Django 4.2.1 on 2023-06-12 18:10

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('ServerSide', '0003_player_locationx_player_locationy_player_role'),
    ]

    operations = [
        migrations.CreateModel(
            name='Guild',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('guildMoney', models.IntegerField(default=0)),
                ('machines', models.IntegerField(default=0)),
            ],
        ),
    ]