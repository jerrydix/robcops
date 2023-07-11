# Generated by Django 4.2.1 on 2023-07-10 22:36

import datetime
from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('ServerSide', '0034_policestation_robunionx_policestation_robuniony_and_more'),
    ]

    operations = [
        migrations.AlterField(
            model_name='breakinevent',
            name='startTime',
            field=models.DateTimeField(default=datetime.datetime(2023, 7, 11, 0, 36, 24, 577832)),
        ),
        migrations.AlterField(
            model_name='player',
            name='rotation',
            field=models.CharField(default='0.0', max_length=100),
        ),
        migrations.AlterField(
            model_name='safe',
            name='timePlaced',
            field=models.DateTimeField(default=datetime.datetime(2023, 7, 11, 0, 36, 24, 576832)),
        ),
    ]