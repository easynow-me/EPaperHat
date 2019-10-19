using System;
using System.Threading;
using Unosquare.RaspberryIO.Abstractions;

namespace EPaperHat
{
    public abstract class BaseEPaper : IEPaper
    {
        protected IEPaperConnection Connection;

        protected int Width;
        protected int Height;

        protected BaseEPaper(IEPaperConnection connection)
        {
            Connection = connection;
        }

        public virtual void Initialize(int width,int height)
        {
            this.Width = width;
            this.Height = height;
            Reset();

            Connection.SendCommand(DeviceCodes.POWER_SETTING);

            //# VDS_EN, VDG_EN
            Connection.SendData(0x03);

            //# VCOM_HV, VGHL_LV[1], VGHL_LV[0]
            Connection.SendData(0x00);

            //# VDH
            Connection.SendData(0x2b);

            //# VDL
            Connection.SendData(0x2b);

            Connection.SendCommand(DeviceCodes.BOOSTER_SOFT_START);
            Connection.SendData(0x17);
            Connection.SendData(0x17);
            Connection.SendData(0x17);

            Connection.SendCommand(DeviceCodes.POWER_ON);
            WaitBusy();

            Connection.SendCommand(DeviceCodes.PANEL_SETTING);

            //KW-BF   KWR-AF    BWROTP 0f
            Connection.SendData(0xbf);

            //KW-BF   KWR-AF    BWROTP 0f
            Connection.SendData(0x0d);

            Connection.SendCommand(DeviceCodes.PLL_CONTROL);

            //3A 100HZ   29 150Hz 39 200HZ    31 171HZ
            Connection.SendData(0x3c);

            Connection.SendCommand(DeviceCodes.RESOLUTION_SETTING);
            Connection.SendData(0x01);
            Connection.SendData(0x90);
            Connection.SendData(0x01);
            Connection.SendData(0x2c);

            Connection.SendCommand(DeviceCodes.VCM_DC_SETTING_REGISTER);
            Connection.SendData(0x28);
            Connection.SendCommand(DeviceCodes.VCOM_AND_DATA_INTERVAL_SETTING);

            //97white border 77black border		VBDF 17|D7 VBDW 97 VBDB 57		VBDF F7 VBDW 77 VBDB 37  VBDR B7
            Connection.SendData(0x97);

            SetLut();
        }

        public virtual void Clear()
        {
            Connection.SendCommand(DeviceCodes.DATA_START_TRANSMISSION_1);
            for (var i = 0; i < Width*Height/8; i++)
            {
                Connection.SendData(0xff);
            }

            Connection.SendCommand(DeviceCodes.DATA_START_TRANSMISSION_2);
            for (var i = 0; i < Width*Height/8; i++)
            {
                Connection.SendData(0xff);
            }

            Connection.SendCommand(DeviceCodes.DISPLAY_REFRESH);
            WaitBusy();
        }

        public virtual void Sleep()
        {
            Connection.SendCommand(DeviceCodes.POWER_OFF);
            WaitBusy();
            Connection.SendCommand(DeviceCodes.DEEP_SLEEP);
            Connection.SendData(0xa5);
        }

        public virtual void ShowImage(byte[] image)
        {
            Connection.SendCommand(DeviceCodes.DATA_START_TRANSMISSION_1);
            for (var i = 0; i < Width*Height/8; i++)
            {
                Connection.SendData(0xff);
            }

            Connection.SendCommand(DeviceCodes.DATA_START_TRANSMISSION_2);
            for (var i = 0; i < Width*Height/8; i++)
            {
                Connection.SendData(image[i]);
            }

            Connection.SendCommand(DeviceCodes.DISPLAY_REFRESH);
            WaitBusy();
        }

        protected virtual void WaitBusy()
        {
            while (!Connection.BusyPin.Read())
            {
                Thread.Sleep(100);
            }
        }

        protected virtual void Reset()
        {
            Connection.RstPin.Write(GpioPinValue.High);
            Thread.Sleep(200);
            Connection.RstPin.Write(GpioPinValue.Low);
            Thread.Sleep(10);
            Connection.RstPin.Write(GpioPinValue.High);
            Thread.Sleep(200);
        }

        protected virtual void SetLut()
        {
            //vcom
            Connection.SendCommand(DeviceCodes.LUT_FOR_VCOM);
            for (var i = 0; i < 44; i++)
            {
                Connection.SendData(DeviceCodes.LutVcomDc[i]);
            }

            //ww --
            Connection.SendCommand(DeviceCodes.LUT_WHITE_TO_WHITE);
            for (var i = 0; i < 42; i++)
            {
                Connection.SendData(DeviceCodes.LutWw[i]);
            }

            //bw r
            Connection.SendCommand(DeviceCodes.LUT_BLACK_TO_WHITE);
            for (var i = 0; i < 42; i++)
            {
                Connection.SendData(DeviceCodes.LutBw[i]);
            }

            //wb w
            Connection.SendCommand(DeviceCodes.LUT_WHITE_TO_BLACK);
            for (var i = 0; i < 42; i++)
            {
                Connection.SendData(DeviceCodes.LutBb[i]);
            }

            //bb b
            Connection.SendCommand(DeviceCodes.LUT_BLACK_TO_BLACK);
            for (var i = 0; i < 42; i++)
            {
                Connection.SendData(DeviceCodes.LutWb[i]);
            }
        }
    }
}