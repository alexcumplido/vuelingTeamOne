﻿namespace VY.Hackathon.TeamOne.WebApi.DataProvider.Entities
{
    public class ResultSnapshot
    {
        public int SnapshotId { get; set; }

        public DateTime SnapshotDateTime { get; set; }

        public string SnapshotDataJson { get; set; } = string.Empty;
    }
}
