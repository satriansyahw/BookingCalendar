﻿namespace BookingCalendar.Utils
{
    public class JwtSettings
    {
        public bool ValidateIssuerSigningKey
        {
            get;
            set;
        }
        public string IssuerSigningKey
        {
            get;
            set;
        }
        public string IssuerSigningKeyRefresh
        {
            get;
            set;
        }
        public bool ValidateIssuer
        {
            get;
            set;
        } = true;
        public string ValidIssuer
        {
            get;
            set;
        }
        public bool ValidateAudience
        {
            get;
            set;
        } = true;
        public string ValidAudience
        {
            get;
            set;
        }
        public bool RequireExpirationTime
        {
            get;
            set;
        }
        public bool ValidateLifetime
        {
            get;
            set;
        } = true;
        public int TokenExpiredInMinutes
        {
            get;
            set;
        }
        public int TokenExpiredInMinutesRefresh
        {
            get;
            set;
        }
    }
}
