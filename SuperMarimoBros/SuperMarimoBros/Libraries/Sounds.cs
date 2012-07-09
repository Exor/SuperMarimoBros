using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace SuperMarimoBros
{
    public class Sounds
    {
        static List<SoundEffect> music;
        static List<SoundEffect> sounds;

        static SoundEffectInstance currentlyPlayingMusic;

        public Sounds()
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

        public enum SoundFx
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

        private void AddSound(SoundEffect sound)
        {
            sounds.Add(sound);
        }

        private void AddMusic(SoundEffect m)
        {
            music.Add(m);
        }

        public static void Play(SoundFx sound)
        {
            sounds[(int)sound].Play();
        }

        public static void Play(Music m)
        {
            if (currentlyPlayingMusic != null)
                currentlyPlayingMusic.Dispose();
            currentlyPlayingMusic = music[(int)m].CreateInstance();
            currentlyPlayingMusic.IsLooped = true;
            currentlyPlayingMusic.Play();
        }

        internal void LoadSounds(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            AddSound(Content.Load<SoundEffect>("SoundFX/blockbreak"));
            AddSound(Content.Load<SoundEffect>("SoundFX/blockhit"));
            AddSound(Content.Load<SoundEffect>("SoundFX/boom"));
            AddSound(Content.Load<SoundEffect>("SoundFX/bowserfall"));
            AddSound(Content.Load<SoundEffect>("SoundFX/bridgebreak"));
            AddSound(Content.Load<SoundEffect>("SoundFX/bulletbill"));
            AddSound(Content.Load<SoundEffect>("SoundFX/coin"));
            AddSound(Content.Load<SoundEffect>("SoundFX/fire"));
            AddSound(Content.Load<SoundEffect>("SoundFX/fireball"));
            AddSound(Content.Load<SoundEffect>("SoundFX/jump"));
            AddSound(Content.Load<SoundEffect>("SoundFX/jumpbig"));
            AddSound(Content.Load<SoundEffect>("SoundFX/mushroomappear"));
            AddSound(Content.Load<SoundEffect>("SoundFX/mushroomeat"));
            AddSound(Content.Load<SoundEffect>("SoundFX/oneup"));
            AddSound(Content.Load<SoundEffect>("SoundFX/pause"));
            AddSound(Content.Load<SoundEffect>("SoundFX/pipe"));
            AddSound(Content.Load<SoundEffect>("SoundFX/scorering"));
            AddSound(Content.Load<SoundEffect>("SoundFX/shot"));
            AddSound(Content.Load<SoundEffect>("SoundFX/shrink"));
            AddSound(Content.Load<SoundEffect>("SoundFX/stomp"));
            AddSound(Content.Load<SoundEffect>("SoundFX/swim"));
            AddSound(Content.Load<SoundEffect>("SoundFX/vine"));

            AddMusic(Content.Load<SoundEffect>("Music/castle-fast"));
            AddMusic(Content.Load<SoundEffect>("Music/castle"));
            AddMusic(Content.Load<SoundEffect>("Music/castleend"));
            AddMusic(Content.Load<SoundEffect>("Music/death"));
            AddMusic(Content.Load<SoundEffect>("Music/gameover"));
            AddMusic(Content.Load<SoundEffect>("Music/intermission"));
            AddMusic(Content.Load<SoundEffect>("Music/levelend"));
            AddMusic(Content.Load<SoundEffect>("Music/lowtime"));
            AddMusic(Content.Load<SoundEffect>("Music/overworld-fast"));
            AddMusic(Content.Load<SoundEffect>("Music/overworld"));
            AddMusic(Content.Load<SoundEffect>("Music/princessmusic"));
            AddMusic(Content.Load<SoundEffect>("Music/starmusic-fast"));
            AddMusic(Content.Load<SoundEffect>("Music/starmusic"));
            AddMusic(Content.Load<SoundEffect>("Music/underground-fast"));
            AddMusic(Content.Load<SoundEffect>("Music/underground"));
            AddMusic(Content.Load<SoundEffect>("Music/underwater-fast"));
            AddMusic(Content.Load<SoundEffect>("Music/underwater"));
        }
    }
}
