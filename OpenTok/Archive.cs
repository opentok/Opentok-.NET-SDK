using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTokSDK
{
    /// <summary>
    /// Defines values returned by the Status property of an Archive object. See the ListArchives()
    /// method of the OpenTok class.
    /// </summary>
    public enum ArchiveStatus
    {
        /// <summary>
        /// The archive file is available for download from the OpenTok cloud. You can get the URL of
        /// the download file by getting the Url property of the Archive object.
        /// </summary>
        AVAILABLE,
        /// <summary>
        /// The archive file has been deleted.
        /// </summary>
        DELETED,
        /// <summary>
        /// The recording of the archive failed.
        /// </summary>
        FAILED,
        /// <summary>
        /// The archive is in progress and no clients are publishing streams to the session.
        /// When an archive is in progress and any client publishes a stream, the status is STARTED.
        /// When an archive is PAUSED, nothing is recorded. When a client starts publishing a stream,
        /// the recording starts (or resumes). If all clients disconnect from a session that is being
        /// archived, the status changes to PAUSED, and after 60 seconds the archive recording stops
        /// (and the status changes to STOPPED).
        /// </summary>
        PAUSED,
        /// <summary>
        /// The archive recording has started and is in progress.
        /// </summary>
        STARTED,
        /// <summary>
        /// The archive recording has stopped, but the file is not available.
        /// </summary>
        STOPPED,
        /// <summary>
        /// The archive is available for download from the the upload targetAmazon S3 bucket or Windows Azure container you set up for your
        /// <a href="https://tokbox.com/account">OpenTok project</a>.
        /// </summary>
        UPLOADED,
        /// <summary>
        /// The archive file is no longer available for download from the OpenTok cloud.
        /// </summary>
        EXPIRED,
        /// <summary>
        /// The status of the archive is unknown.
        /// </summary>
        UNKOWN
    }

    /// <summary>
    /// Defines values for the OutputMode property of an Archive object.
    /// </summary>
    public enum OutputMode
    {
        /// <summary>
        /// All streams in the archive are recorded to a single (composed) file.
        /// </summary>
        COMPOSED,
        /// <summary>
        /// Each stream in the archive is recorded to its own individual file.
        /// </summary>
        INDIVIDUAL
    }

    /// <summary>
    /// Represents an archive of an OpenTok session.
    /// </summary>
    public class Archive
    {

        private OpenTok opentok;

        /// <summary>
        /// Initializes a new instance of the <see cref="Archive"/> class.
        /// </summary>
        protected Archive()
        {
        }

        internal Archive(OpenTok opentok)
        {
            this.opentok = opentok;
        }

        internal void CopyArchive(Archive archive)
        {
            this.CreatedAt = archive.CreatedAt;
            this.Duration = archive.Duration;
            this.Id = archive.Id;
            this.Name = archive.Name;
            this.PartnerId = archive.PartnerId;
            this.SessionId = archive.SessionId;
            this.Size = archive.Size;
            this.Status = archive.Status;
            this.Url = archive.Url;
            this.Password = archive.Password;
            this.HasVideo = archive.HasVideo;
            this.HasAudio = archive.HasAudio;
            this.OutputMode = archive.OutputMode;
            this.Resolution = archive.Resolution;
        }

        /// <summary>
        /// The time at which the archive was created, in milliseconds since the Unix epoch.
        /// </summary>
        public long CreatedAt { get; set; }

        /// <summary>
        /// The duration of the archive, in milliseconds.
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// The archive ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the archive.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The OpenTok API key associated with the archive.
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// The session ID of the OpenTok session associated with this archive.
        /// </summary>
        public String SessionId { get; set; }

        /// <summary>
        /// For archives with the status ArchiveStatus.STOPPED or ArchiveStatus.FAILED, this string
        /// describes the reason the archive stopped (such as "maximum duration exceeded") or failed.
        /// </summary>
        public String Reason { get; set; }

        /// <summary>
        /// Whether the archive includes a video track (true) or not (false).
        /// </summary>
        public bool HasVideo { get; set; }

        /// <summary>
        /// Whether the archive includes an audio track (true) or not (false).
        /// </summary>
        public bool HasAudio { get; set; }

        /// <summary>
        /// The resolution of the archive.
        /// </summary>
        public string Resolution { get; set; }

        /// <summary>
        /// Whether all streams in the archive are recorded to a single file
        /// (<see cref="OutputMode.COMPOSED"/>) or to individual files
        /// (<see cref="OutputMode.INDIVIDUAL"/>). To record streams to individual
        /// files, pass <see cref="OutputMode.INDIVIDUAL"/> as the <see cref="OutputMode"/>
        /// parameter when calling the <see cref="OpenTok.StartArchive"/> method.
        /// </summary>
        public OutputMode OutputMode { get; set; }

        /// <summary>
        /// The size of the MP4 file. For archives that have not been generated, this value is set
        /// to 0. We use long instead of int to support archives larger than 2GB.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// The status of the archive, as defined by the ArchiveStatus enum.
        /// </summary>
        public ArchiveStatus Status { get; set; }

        /// <summary>
        /// The download URL of the available MP4 file. This is only set for an archive with the
        /// status set to ArchiveStatus.AVAILABLE; for other archives, (including archives with the
        /// status of ArchiveStatus.UPLOADED) this method returns null. The download URL is
        /// obfuscated, and the file is only available from the URL for 10 minutes. To generate a
        /// new URL, call the ListArchives() or GetArchive() method of the OpenTok object.
        /// </summary>
        public String Url { get; set; }

        /// <summary>
        /// The encryption password of the archive.
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// Stops the OpenTok archive if it is being recorded.
        /// <para>
        /// Archives automatically stop recording after 120 minutes or when all clients have
        /// disconnected from the session being archived.
        /// </para>
        /// </summary>
        public void Stop()
        {
            if (opentok != null)
            {
                Archive archive = opentok.StopArchive(Id.ToString());
                Status = archive.Status;
            }
        }

        /// <summary>
        /// Deletes the OpenTok archive.
        /// <para>
        /// You can only delete an archive which has a status of "available" or "uploaded". Deleting
        /// an archive removes its record from the list of archives. For an "available" archive, it
        /// also removes the archive file, making it unavailable for download.
        /// </para>
        /// </summary>
        public void Delete()
        {
            if (opentok != null)
            {
                opentok.DeleteArchive(Id.ToString());
            }
        }
    }
}
