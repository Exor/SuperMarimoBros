using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace XnaLibrary
{
    public class SoundManager
    {
        List<SoundEffect> music;
        List<SoundEffect> sounds;

        SoundEffectInstance currentlyPlayingMusic;

        public SoundManager()
        {
            music = new List<SoundEffect>();
            sounds = new List<SoundEffect>();
        }

        public enum Music
        {
            castlefast,
            castle,
            castleend,
            death,
            gameover,
            intermission,
            levelend,
            lowtime,
            overworldfast,
            overworld,
            princessmusic,
            starmusicfast,
            starmusic,
            undergroundfast,
            underground,
            underwaterfast,
            underwater
        };

        public enum Sound
        {
            blockbreak,
            blockhit,
            boom,
            bowserfall,
            bridgebreak,
            bulletbill,
            coin,
            fire,
            fireball,
            jump,
            jumpbig,
            mushroomappear,
            mushroomeat,
            oneup,
            pause,
            pipe,
            scrorering,
            shot,
            shrink,
            stomp,
            swim,
            vine
        };

        public void AddSound(SoundEffect sound)
        {
            sounds.Add(sound);
        }

        public void AddMusic(SoundEffect m)
        {
            music.Add(m);
        }

        public void Play(Sound sound)
        {
            sounds[(int)sound].Play();
        }

        public void Play(Music m)
        {
            if (currentlyPlayingMusic != null)
                currentlyPlayingMusic.Dispose();
            currentlyPlayingMusic = music[(int)m].CreateInstance();
            currentlyPlayingMusic.IsLooped = true;
            currentlyPlayingMusic.Play();
        }
    }
}
