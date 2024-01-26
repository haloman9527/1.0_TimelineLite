using System;
using System.Collections.Generic;
using UnityEngine;

namespace CZToolKit.TimelineLite
{
    public class Graph
    {
        public float length;
        public List<Group> groups;
    }

    public class Group
    {
        public bool isActive;
        public List<Track> tracks;
    }

    public class Track
    {
        public bool isActive;
        public List<Clip> clips;
    }

    public class Clip
    {
        public bool isActive;
        public float startTime;
        public float endTime;
    }

    public class GraphBehavior
    {
        private Graph data;

        private bool isActive;
        private float currentTime;

        private List<GroupBehavior> groups;

        public bool IsActive
        {
            get { return isActive; }
        }

        public float Length
        {
            get { return data.length; }
        }

        public float CurrentTime
        {
            get { return currentTime; }
        }

        public GraphBehavior(Graph data)
        {
            this.data = data;
            if (data.groups != null)
            {
                groups = new List<GroupBehavior>(data.groups.Count);
                for (int i = 0; i < data.groups.Count; i++)
                {
                    var group = data.groups[i];
                    var groupBehaviorType = ViewModelFactory.GetViewModelType(group.GetType());
                    var groupBehavior = ObjectPool.Instance.Spawn(groupBehaviorType) as GroupBehavior;
                    groupBehavior.SetUp(group, this);
                    groups.Add(groupBehavior);
                }
            }
        }

        public void Play()
        {
            Play(0, data.length, null);
        }

        public void Play(float startTime)
        {
            Play(startTime, data.length, null);
        }

        public void Play(float startTime, Action stopCallback)
        {
            Play(startTime, data.length, stopCallback);
        }

        public void Play(float startTime, float endTime, Action stopCallback)
        {
            if (startTime < 0 || startTime > data.length || endTime < 0 || endTime > data.length)
            {
                return;
            }

            if (startTime > endTime)
            {
                return;
            }

            if (isActive)
            {
                return;
            }

            currentTime = startTime;

            Sample(0);
        }

        public void Stop()
        {
            if (!isActive)
            {
                return;
            }

            isActive = false;
            currentTime = 0;
        }

        public void Sample(float time)
        {
            time = Mathf.Clamp(time, 0, Length);
            var previousTime = currentTime;
            this.currentTime = time;
            if (groups == null)
                return;
            foreach (var group in groups)
            {
                if (group.IsActive)
                    continue;

                if (Math.Abs(time - previousTime) > 0.0001f && !group.IsTriggered && time >= group.StartTime && time <= group.EndTime)
                    group.Enter();

                if (time >= group.StartTime && time <= group.EndTime)
                    group.Update(time, previousTime);

                if (Math.Abs(time - previousTime) > 0.0001f && group.IsTriggered)
                {
                    if (time >= previousTime && time >= group.EndTime)
                        group.Exit();
                    else if (time < previousTime && time <= group.StartTime)
                        group.Exit();
                }
            }
        }
    }

    public class GroupBehavior
    {
        private Group data;
        private List<TrackBehavior> tracks;
        private bool isTriggered;

        public GraphBehavior Root { get; private set; }

        public bool IsActive
        {
            get { return data.isActive; }
        }

        public float StartTime
        {
            get { return 0; }
        }

        public float EndTime
        {
            get { return Root.Length; }
        }

        public bool IsTriggered
        {
            get { return isTriggered; }
        }

        public void SetUp(Group data, GraphBehavior graph)
        {
            this.data = data;
            this.Root = graph;
            if (data.tracks != null)
            {
                this.tracks = new List<TrackBehavior>(data.tracks.Count);
                for (int i = 0; i < data.tracks.Count; i++)
                {
                    var track = data.tracks[i];
                    var trackBehaviorType = ViewModelFactory.GetViewModelType(track.GetType());
                    var trackBehavior = ObjectPool.Instance.Spawn(trackBehaviorType) as TrackBehavior;
                    trackBehavior.SetUp(track, this);
                    tracks.Add(trackBehavior);
                }
            }
        }

        public void Enter()
        {
            isTriggered = true;

            if (tracks == null)
            {
                return;
            }

            foreach (var track in tracks)
            {
                if (track.IsActive)
                    track.Enter();
            }
        }

        public void Exit()
        {
            isTriggered = false;

            if (tracks == null)
            {
                return;
            }

            foreach (var track in tracks)
            {
                if (!track.IsActive)
                    continue;

                if (track.IsTriggered)
                    track.Exit();
            }
        }

        public void Update(float time, float previousTime)
        {
            if (tracks == null)
            {
                return;
            }

            foreach (var track in tracks)
            {
                if (!track.IsActive)
                    continue;

                if (Math.Abs(time - previousTime) > 0.0001f && !track.IsTriggered && time >= track.StartTime && time <= track.EndTime)
                    track.Enter();

                if (time >= track.StartTime && time <= track.EndTime)
                    track.Update(time, previousTime);

                if (Math.Abs(time - previousTime) > 0.0001f && track.IsTriggered)
                {
                    if (time >= previousTime && time >= track.EndTime)
                        track.Exit();
                    else if (time < previousTime && time <= track.StartTime)
                        track.Exit();
                }
            }
        }
    }

    public class TrackBehavior
    {
        private Track data;
        private List<ClipBehavior> clips;
        private bool isTriggered;

        public GroupBehavior Parent { get; private set; }

        public GraphBehavior Root { get; private set; }

        public bool IsActive
        {
            get { return data.isActive; }
        }

        public float StartTime
        {
            get { return 0; }
        }

        public float EndTime
        {
            get { return Root.Length; }
        }

        public bool IsTriggered
        {
            get { return isTriggered; }
        }

        public void SetUp(Track data, GroupBehavior group)
        {
            this.data = data;
            this.Parent = group;
            this.Root = group.Root;
            if (data.clips != null)
            {
                this.clips = new List<ClipBehavior>(data.clips.Count);
                for (int i = 0; i < data.clips.Count; i++)
                {
                    var clip = data.clips[i];
                    var clipBehaviorType = ViewModelFactory.GetViewModelType(clip.GetType());
                    var clipBehavior = ObjectPool.Instance.Spawn(clipBehaviorType) as ClipBehavior;
                    clipBehavior.SetUp(clip, this);
                    clips.Add(clipBehavior);
                }
            }
        }

        public void Enter()
        {
            isTriggered = true;
            OnEnter();
        }

        public void Exit()
        {
            isTriggered = false;
            OnExit();
            if (clips == null)
            {
                return;
            }

            foreach (var clip in clips)
            {
                if (!clip.IsActive)
                    continue;

                if (clip.IsTriggerd)
                    clip.Exit();
            }
        }

        public void Update(float time, float previousTime)
        {
            OnUpdate(time, previousTime);

            if (clips == null)
            {
                return;
            }

            if (time >= previousTime)
            {
                foreach (var clip in clips)
                {
                    if (!clip.IsActive)
                        continue;

                    if (Math.Abs(time - previousTime) > 0.0001f && !clip.IsTriggerd && time >= clip.StartTime && time <= clip.EndTime)
                        clip.Enter();

                    if (time >= clip.StartTime && time >= clip.EndTime)
                        clip.Update(time, Mathf.Clamp(previousTime, StartTime, EndTime));

                    if (Math.Abs(time - previousTime) > 0.0001f && clip.IsTriggerd && time >= clip.EndTime)
                        clip.Exit();
                }
            }
            else
            {
                foreach (var clip in clips)
                {
                    if (!clip.IsActive)
                        continue;

                    if (Math.Abs(time - previousTime) > 0.0001f && !clip.IsTriggerd && time >= clip.StartTime && time <= clip.EndTime)
                        clip.ReverseEnter();

                    if (time >= clip.StartTime && time >= clip.EndTime)
                        clip.Update(time, Mathf.Clamp(previousTime, StartTime, EndTime));

                    if (Math.Abs(time - previousTime) > 0.0001f && clip.IsTriggerd && time <= clip.StartTime)
                        clip.Exit();
                }
            }
        }

        protected virtual void OnEnter()
        {
        }

        protected virtual void OnExit()
        {
        }

        protected virtual void OnUpdate(float time, float previousTime)
        {
        }
    }

    public class ClipBehavior
    {
        private Clip data;
        private bool isTriggered;

        public TrackBehavior Parent { get; private set; }

        public GraphBehavior Root { get; private set; }

        public bool IsActive
        {
            get { return data.isActive; }
        }

        public float StartTime
        {
            get { return 0; }
        }

        public float EndTime
        {
            get { return Root.Length; }
        }

        public bool IsTriggerd
        {
            get { return isTriggered; }
        }

        public void SetUp(Clip data, TrackBehavior track)
        {
            this.data = data;
            this.Parent = track;
            this.Root = track.Root;
        }

        public void Enter()
        {
            isTriggered = true;
            OnEnter();
        }

        public void ReverseEnter()
        {
            isTriggered = true;
            OnReverseEnter();
        }

        public void Exit()
        {
            isTriggered = false;
            OnExit();
        }

        public void ReverseExit()
        {
            isTriggered = false;
            OnReverseExit();
        }

        public void Update(float time, float previousTime)
        {
            OnUpdate(time, previousTime);
        }

        protected virtual void OnEnter()
        {
        }

        protected virtual void OnReverseEnter()
        {
        }

        protected virtual void OnExit()
        {
        }

        protected virtual void OnReverseExit()
        {
        }

        protected virtual void OnUpdate(float time, float previousTime)
        {
        }
    }
}