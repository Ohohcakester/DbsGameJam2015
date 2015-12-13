using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = OhRandom;

namespace Orb {
    public class Event{
        public string eventName;
        public string eventDescription;

        public int probability100 { get; private set; }
        public Alignment alignment { get; private set; }

        public string ProbabilityString
        {
            get { return probability100 + "%"; }
        }

        private OrbEventEnumerator.Event eventType;
	    public bool hasExpired { get; private set; }

	    //Constructor
	    public Event (OrbEventEnumerator.Event ev)
	    {
		    eventType = ev;
		    eventDescription = getEventDescription(eventType);
		    eventName = eventDescription;
		    hasExpired = false;
	        probability100 = GenerateProbability(ev);
	        alignment = eventAlignment(ev);
	    }

	    public void expire(){
		    hasExpired = true;
	    }

	    public void configureUIEventObject(GameObject eventObj) {
    //		Debug.Log ("Configuring" + eventDescription);
            eventObj.transform.Find("TextCanvas").GetComponentInChildren<Text>().text = eventDescription;
            eventObj.transform.Find("ProbabilityText").GetComponentInChildren<Text>().text = ProbabilityString;
            eventObj.transform.Find("EventPlank").GetComponent<SpriteRenderer>().color = PanelColor();
	        eventObj.transform.Find("EventIcon").GetComponent<EventIcon>().SetSprite(eventType);
	    }

        private Color PanelColor()
        {
            switch (alignment)
            {
                case Alignment.BAD: return new Color(1, 0.5f, 0.5f);
                case Alignment.GOOD: return new Color(0.25f, 1f, 0.5f);
                case Alignment.NEUTRAL: return Color.white;
            }
            return Color.white;
        }

        public OrbEventEnumerator.Event getEventType() {
		    return this.eventType;
	    }

	    public OrbEventEnumerator.Event getRandomEvent(){
		    return OrbEventEnumerator.Event.None;
	    }

        public static string getEventDescription(OrbEventEnumerator.Event ev)
        {
            switch (ev)
            {
                case OrbEventEnumerator.Event.BonusStarlight: return "Treasure!";
                case OrbEventEnumerator.Event.Multiplier2: return "2x Orb\nMultiplier";
                case OrbEventEnumerator.Event.Multiplier3: return "3x Orb\nMultiplier";
                case OrbEventEnumerator.Event.LessJellyfish: return "Less\nJellyfish";
                case OrbEventEnumerator.Event.Multiplier5: return "5x Orb\nMultiplier";
                case OrbEventEnumerator.Event.EnemiesRun: return "Terrified\nJellyfish";
                case OrbEventEnumerator.Event.LessCrabs: return "Less\nCrabs";
                case OrbEventEnumerator.Event.MoreStarlight: return "Abundant\nStarlight";

                case OrbEventEnumerator.Event.SquidInk: return "Squid\nSplatter";
                case OrbEventEnumerator.Event.FastJellyfish: return "Speedy\nJellyfish";
                case OrbEventEnumerator.Event.MoreJellyfish: return "Jellyfish\nSeason";
                case OrbEventEnumerator.Event.FastCrabs: return "Speedy\nCrabs";
                case OrbEventEnumerator.Event.MoreCrabs: return "Crab\nSeason";
                case OrbEventEnumerator.Event.UndercurrentLeft: return "Fast\nCurrent";
                case OrbEventEnumerator.Event.UndercurrentRight: return "Fast\nCurrent";
                case OrbEventEnumerator.Event.AggressiveJellyfish: return "They can\nsee you";
                case OrbEventEnumerator.Event.Multiplier0_2: return "0.2x Orb\nMultiplier";
                case OrbEventEnumerator.Event.Multiplier0_5: return "0.5x Orb\nMultiplier";
                case OrbEventEnumerator.Event.LessStarlight: return "Starlight\nScarcity";
                case OrbEventEnumerator.Event.Multiplier0_8: return "0.8x Orb\nMultiplier";
            }
            return ev.ToString();
        }

        public static int GenerateProbability(OrbEventEnumerator.Event ev)
        {
            return eventProbabilityPercent(ev) + Random.Range(0, 3)*10;
        }

        private static int eventProbabilityPercent(OrbEventEnumerator.Event ev)
        {
            switch (ev)
            {
                case OrbEventEnumerator.Event.BonusStarlight: return 60;
                case OrbEventEnumerator.Event.Multiplier2: return 70;
                case OrbEventEnumerator.Event.Multiplier3: return 60;
                case OrbEventEnumerator.Event.LessJellyfish: return 40;
                case OrbEventEnumerator.Event.Multiplier5: return 50;
                case OrbEventEnumerator.Event.EnemiesRun: return 40;
                case OrbEventEnumerator.Event.LessCrabs: return 50;
                case OrbEventEnumerator.Event.MoreStarlight: return 30;

                case OrbEventEnumerator.Event.SquidInk: return 40;
                case OrbEventEnumerator.Event.FastJellyfish: return 40;
                case OrbEventEnumerator.Event.MoreJellyfish: return 30;
                case OrbEventEnumerator.Event.FastCrabs: return 50;
                case OrbEventEnumerator.Event.MoreCrabs: return 30;
                case OrbEventEnumerator.Event.UndercurrentLeft: return 60;
                case OrbEventEnumerator.Event.UndercurrentRight: return 60;
                case OrbEventEnumerator.Event.AggressiveJellyfish: return 40;
                case OrbEventEnumerator.Event.Multiplier0_2: return 40;
                case OrbEventEnumerator.Event.Multiplier0_5: return 50;
                case OrbEventEnumerator.Event.LessStarlight: return 70;
                case OrbEventEnumerator.Event.Multiplier0_8: return 60;
            }
            return 50;
        }

        public static Alignment eventAlignment(OrbEventEnumerator.Event ev)
        {
            switch (ev)
            {
                case OrbEventEnumerator.Event.BonusStarlight: return Alignment.GOOD;
                case OrbEventEnumerator.Event.Multiplier2: return Alignment.GOOD;
                case OrbEventEnumerator.Event.Multiplier3: return Alignment.GOOD;
                case OrbEventEnumerator.Event.LessJellyfish: return Alignment.GOOD;
                case OrbEventEnumerator.Event.Multiplier5: return Alignment.GOOD;
                case OrbEventEnumerator.Event.EnemiesRun: return Alignment.GOOD;
                case OrbEventEnumerator.Event.LessCrabs: return Alignment.GOOD;
                case OrbEventEnumerator.Event.MoreStarlight: return Alignment.GOOD;

                case OrbEventEnumerator.Event.SquidInk: return Alignment.BAD;
                case OrbEventEnumerator.Event.FastJellyfish: return Alignment.BAD;
                case OrbEventEnumerator.Event.MoreJellyfish: return Alignment.BAD;
                case OrbEventEnumerator.Event.FastCrabs: return Alignment.BAD;
                case OrbEventEnumerator.Event.MoreCrabs: return Alignment.BAD;
                case OrbEventEnumerator.Event.UndercurrentLeft: return Alignment.NEUTRAL;
                case OrbEventEnumerator.Event.UndercurrentRight: return Alignment.NEUTRAL;
                case OrbEventEnumerator.Event.AggressiveJellyfish: return Alignment.BAD;
                case OrbEventEnumerator.Event.Multiplier0_2: return Alignment.BAD;
                case OrbEventEnumerator.Event.Multiplier0_5: return Alignment.BAD;
                case OrbEventEnumerator.Event.LessStarlight: return Alignment.BAD;
                case OrbEventEnumerator.Event.Multiplier0_8: return Alignment.BAD;
            }
            return Alignment.NEUTRAL;
        }

    }

    public enum Alignment
    {
        GOOD,BAD,NEUTRAL
    }
}
