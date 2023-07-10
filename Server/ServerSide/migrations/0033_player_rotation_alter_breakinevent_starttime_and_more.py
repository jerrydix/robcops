# Generated by Django 4.2.1 on 2023-07-10 18:25

import datetime
from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('ServerSide', '0032_alter_breakinevent_starttime_and_more'),
    ]

    operations = [
        migrations.AddField(
            model_name='player',
            name='rotation',
            field=models.CharField(default='0.0', max_length=30),
        ),
        migrations.AlterField(
            model_name='breakinevent',
            name='startTime',
            field=models.DateTimeField(default=datetime.datetime(2023, 7, 10, 20, 25, 35, 974182)),
        ),
        migrations.AlterField(
            model_name='safe',
            name='timePlaced',
            field=models.DateTimeField(default=datetime.datetime(2023, 7, 10, 20, 25, 35, 974182)),
        ),
    ]