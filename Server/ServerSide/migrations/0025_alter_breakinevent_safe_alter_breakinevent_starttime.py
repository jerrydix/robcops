# Generated by Django 4.2.1 on 2023-07-03 11:25

import datetime
from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    dependencies = [
        ('ServerSide', '0024_breakinevent_alarms_breakinevent_c4s_and_more'),
    ]

    operations = [
        migrations.AlterField(
            model_name='breakinevent',
            name='safe',
            field=models.OneToOneField(default=None, on_delete=django.db.models.deletion.CASCADE, primary_key=True, serialize=False, to='ServerSide.safe'),
        ),
        migrations.AlterField(
            model_name='breakinevent',
            name='startTime',
            field=models.DateTimeField(default=datetime.datetime(2023, 7, 3, 13, 25, 29, 980256)),
        ),
    ]
