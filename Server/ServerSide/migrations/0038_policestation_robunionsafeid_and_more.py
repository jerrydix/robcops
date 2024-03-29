# Generated by Django 4.2.1 on 2023-07-12 22:36

import datetime
from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('ServerSide', '0037_alter_breakinevent_starttime_alter_safe_isrobunion_and_more'),
    ]

    operations = [
        migrations.AddField(
            model_name='policestation',
            name='robUnionSafeID',
            field=models.IntegerField(blank=True, default=-1),
        ),
        migrations.AlterField(
            model_name='breakinevent',
            name='startTime',
            field=models.DateTimeField(default=datetime.datetime(2023, 7, 13, 0, 36, 1, 945809)),
        ),
        migrations.AlterField(
            model_name='safe',
            name='timePlaced',
            field=models.DateTimeField(default=datetime.datetime(2023, 7, 13, 0, 36, 1, 945809)),
        ),
    ]
