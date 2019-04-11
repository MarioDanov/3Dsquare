// CameraLibrary.h

#pragma once

using namespace System;
using namespace System::Threading;

namespace CameraLibrary {

	public delegate void CameraCallback(int x, int y, int width, int height, bool available);
	public delegate void CameraColorCallback(int h, int s, int v);

	public ref class CameraWorker
	{
	private:
		bool windowOpened = false;
		bool working = false;
		int ch = 60, cs = 100, cv = 100, dch = 25, dcs = 50, dcv = 80;
		int bh = 175, bs = 170, bv = 180, dbh = 20, dbs = 110, dbv = 100;
	private:
		CameraCallback ^cameraCallback;
		CameraColorCallback ^colorCallback = nullptr;
		Thread ^thread;
		void Work();
	public:
		void StartCamera(CameraCallback ^callback);
		void StopCamera();
		void OpenWindow(long windowHandle);
		void CloseWindow();
		void Config(int ch, int cs, int cv, int dch, int dcs, int dcv,
			int bh, int bs, int bv, int dbh, int dbs, int dbv);
		void GetConfig(int %ch, int %cs, int %cv, int %dch, int %dcs, int %dcv,
			int %bh, int %bs, int %bv, int %dbh, int %dbs, int %dbv);
		void SetColorCallback(CameraColorCallback ^colorCallback);
	};
}
