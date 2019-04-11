// CameraTest.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "opencv2/opencv.hpp"
#include <iostream>

using namespace cv;
using namespace std;

int mouseX = 0;
int mouseY = 0;
bool record = false;

void mouse_callback(int  event, int  x, int  y, int  flag, void *param)
{
	if (event == EVENT_LBUTTONDOWN)
	{
		record = true;
	}
	if (event == EVENT_MOUSEMOVE) {
		mouseX = x;
		mouseY = y;
	}
}

struct pt {
	int x;
	int y;
};

pt pts[2100000];

int main()
{
	VideoCapture cap(0); // open the default camera
	if (!cap.isOpened())  // check if we succeeded
		return -1;
	cap.set(CV_CAP_PROP_FRAME_WIDTH, 1920);
	cap.set(CV_CAP_PROP_FRAME_HEIGHT, 1080);
	Mat edges;
	namedWindow("edges", 1);
	setMouseCallback("edges", mouse_callback);
	for (;;)
	{
		Mat frame;
		cap >> frame; // get a new frame from camera
		cvtColor(frame, edges, COLOR_BGR2HSV);
		int cols = frame.cols;
		int rows = frame.rows;
		unsigned char *eptr = edges.data;
		unsigned char *ptr = frame.data;
		unsigned int h = eptr[mouseY * cols * 3 + mouseX * 3 + 0];
		unsigned int s = eptr[mouseY * cols * 3 + mouseX * 3 + 1];
		unsigned int v = eptr[mouseY * cols * 3 + mouseX * 3 + 2];
		int th = 60;
		int ts = 100;
		int tv = 100;
		int dh = 25;
		int ds = 50;
		int dv = 80;
		int th2 = 175;
		int ts2 = 170;
		int tv2 = 180;
		int dh2 = 20;
		int ds2 = 110;
		int dv2 = 100;
		if (record)
		{
			cout << h << " " << s << " " << v << endl;
			record = false;
		}
		int ptsSize = 0;
		int Max_ptsSize = 0;
		int Max_ptsSize_coordinateX = 0;
		int Max_ptsSize_coordinateY = 0;
		unsigned char *ceptr = eptr;

		for (int y = 0; y < rows; y++)
		{
			for (int x = 0; x < cols; x++)
			{

				int i = 0;
				int h = *(ceptr++);
				int s = *(ceptr++);
				int v = *(ceptr++);
				if ((abs(ts - s) < ds) && (abs(th - h) < dh) && (abs(tv - v) < dv))
				{
					*(ceptr - 3) = 0;

					pts[0].x = x;
					pts[0].y = y;
					ptsSize = 1;
					while (i < ptsSize)
					{
						pt p = pts[i];
						i++;
						if (p.x > 0)
						{
							h = eptr[p.y * cols * 3 + (p.x - 1) * 3 + 0];
							s = eptr[p.y * cols * 3 + (p.x - 1) * 3 + 1];
							v = eptr[p.y * cols * 3 + (p.x - 1) * 3 + 2];
							if ((abs(ts - s) < ds) && (abs(th - h) < dh) && (abs(tv - v) < dv))
							{
								pts[ptsSize].x = p.x - 1;
								pts[ptsSize].y = p.y;
								ptsSize++;
								eptr[p.y * cols * 3 + (p.x - 1) * 3 + 0] = 0;
								eptr[p.y * cols * 3 + (p.x - 1) * 3 + 1] = 0;
								eptr[p.y * cols * 3 + (p.x - 1) * 3 + 2] = 0;
							}
						}
						if (p.x < cols - 1)
						{
							h = eptr[p.y * cols * 3 + (p.x + 1) * 3 + 0];
							s = eptr[p.y * cols * 3 + (p.x + 1) * 3 + 1];
							v = eptr[p.y * cols * 3 + (p.x + 1) * 3 + 2];
							if ((abs(ts - s) < ds) && (abs(th - h) < dh) && (abs(tv - v) < dv))
							{
								pts[ptsSize].x = p.x + 1;
								pts[ptsSize].y = p.y;
								ptsSize++;
								eptr[p.y * cols * 3 + (p.x + 1) * 3 + 0] = 0;
								eptr[p.y * cols * 3 + (p.x + 1) * 3 + 1] = 0;
								eptr[p.y * cols * 3 + (p.x + 1) * 3 + 2] = 0;
							}
						}
						if (p.y > 0)
						{
							h = eptr[(p.y - 1) * cols * 3 + p.x * 3 + 0];
							s = eptr[(p.y - 1) * cols * 3 + p.x * 3 + 1];
							v = eptr[(p.y - 1) * cols * 3 + p.x * 3 + 2];
							if ((abs(ts - s) < ds) && (abs(th - h) < dh) && (abs(tv - v) < dv))
							{
								pts[ptsSize].x = p.x;
								pts[ptsSize].y = p.y - 1;
								ptsSize++;
								eptr[(p.y - 1) * cols * 3 + p.x * 3 + 0] = 0;
								eptr[(p.y - 1) * cols * 3 + p.x * 3 + 1] = 0;
								eptr[(p.y - 1)* cols * 3 + p.x * 3 + 2] = 0;
							}
						}
						if (p.y < rows - 1)
						{
							h = eptr[(p.y + 1) * cols * 3 + p.x * 3 + 0];
							s = eptr[(p.y + 1) * cols * 3 + p.x * 3 + 1];
							v = eptr[(p.y + 1) * cols * 3 + p.x * 3 + 2];
							if ((abs(ts - s) < ds) && (abs(th - h) < dh) && (abs(tv - v) < dv))
							{
								pts[ptsSize].x = p.x;
								pts[ptsSize].y = p.y + 1;
								ptsSize++;
								eptr[(p.y + 1) * cols * 3 + p.x * 3 + 0] = 0;
								eptr[(p.y + 1) * cols * 3 + p.x * 3 + 1] = 0;
								eptr[(p.y + 1)* cols * 3 + p.x * 3 + 2] = 0;
							}
						}
					}

					if (ptsSize > 30)
					{
						int point = 0;
						int minX = pts[0].x;
						int maxX = pts[0].x;
						int minY = pts[0].y;
						int maxY = pts[0].y;
						int difX, difY;
						double circlsS;
						while (point < ptsSize)
						{
							if (pts[point].x < minX)
							{
								minX = pts[point].x;
							}
							else if (pts[point].x > maxX)
							{
								maxX = pts[point].x;
							}
							else if (pts[point].y < minY) 
							{
								minY = pts[point].y;
							}
							else if (pts[point].y > maxY)
							{
								maxY = pts[point].y;
							}
							ptr[pts[point].y * cols * 3 + pts[point].x * 3 + 0] = 0;
							ptr[pts[point].y * cols * 3 + pts[point].x * 3 + 1] = 255;
							ptr[pts[point].y * cols * 3 + pts[point].x * 3 + 2] = 0;
							point++;
						}
						difX = maxX - minX;
						difY = maxY - minY;
						if (difX > difY) 
						{
							circlsS = pow(difX / 2, 2) * 3.14;
						}
						else 
						{
							circlsS = pow(difY / 2, 2) * 3.14;
						}
						double k =  ptsSize / circlsS;
						//cout << k << " " << ptsSize << endl;
						if (k > 0.5) {

							int oldPtsSize = ptsSize;
							int i2 = 0;
							int h2 = 0;
							int s2 = 0;
							int v2 = 0;

							int d1 = 8;
							int d2 = 20;
							for (int i = 0; i < d1; i++)
							{
								int ptsSizeTmp = ptsSize;
								i2 = 0;
								while (i2 < ptsSizeTmp)
								{
									pt p2 = pts[i2];
									i2++;
									if (p2.x > 0)
									{
										h2 = eptr[p2.y * cols * 3 + (p2.x - 1) * 3 + 0];
										s2 = eptr[p2.y * cols * 3 + (p2.x - 1) * 3 + 1];
										v2 = eptr[p2.y * cols * 3 + (p2.x - 1) * 3 + 2];
										if ((h2 != 0) || (s2 != 0) || (v2 != 0))
										{
											pts[ptsSize].x = p2.x - 1;
											pts[ptsSize].y = p2.y;
											ptsSize++;
											eptr[p2.y * cols * 3 + (p2.x - 1) * 3 + 0] = 0;
											eptr[p2.y * cols * 3 + (p2.x - 1) * 3 + 1] = 0;
											eptr[p2.y * cols * 3 + (p2.x - 1) * 3 + 2] = 0;
										}
									}
									if (p2.x < cols - 1)
									{
										h2 = eptr[p2.y * cols * 3 + (p2.x + 1) * 3 + 0];
										s2 = eptr[p2.y * cols * 3 + (p2.x + 1) * 3 + 1];
										v2 = eptr[p2.y * cols * 3 + (p2.x + 1) * 3 + 2];
										if ((h2 != 0) || (s2 != 0) || (v2 != 0))
										{
											pts[ptsSize].x = p2.x + 1;
											pts[ptsSize].y = p2.y;
											ptsSize++;
											eptr[p2.y * cols * 3 + (p2.x + 1) * 3 + 0] = 0;
											eptr[p2.y * cols * 3 + (p2.x + 1) * 3 + 1] = 0;
											eptr[p2.y * cols * 3 + (p2.x + 1) * 3 + 2] = 0;
										}
									}
									if (p2.y > 0)
									{
										h2 = eptr[(p2.y - 1) * cols * 3 + p2.x * 3 + 0];
										s2 = eptr[(p2.y - 1) * cols * 3 + p2.x * 3 + 1];
										v2 = eptr[(p2.y - 1) * cols * 3 + p2.x * 3 + 2];
										if ((h2 != 0) || (s2 != 0) || (v2 != 0))
										{
											pts[ptsSize].x = p2.x;
											pts[ptsSize].y = p2.y - 1;
											ptsSize++;
											eptr[(p2.y - 1) * cols * 3 + p2.x * 3 + 0] = 0;
											eptr[(p2.y - 1) * cols * 3 + p2.x * 3 + 1] = 0;
											eptr[(p2.y - 1) * cols * 3 + p2.x * 3 + 2] = 0;
										}
									}
									if (p2.y < rows - 1)
									{
										h2 = eptr[(p2.y + 1) * cols * 3 + p2.x * 3 + 0];
										s2 = eptr[(p2.y + 1) * cols * 3 + p2.x * 3 + 1];
										v2 = eptr[(p2.y + 1) * cols * 3 + p2.x * 3 + 2];
										if ((h2 != 0) || (s2 != 0) || (v2 != 0))
										{
											pts[ptsSize].x = p2.x;
											pts[ptsSize].y = p2.y + 1;
											ptsSize++;
											eptr[(p2.y + 1) * cols * 3 + p2.x * 3 + 0] = 0;
											eptr[(p2.y + 1) * cols * 3 + p2.x * 3 + 1] = 0;
											eptr[(p2.y + 1) * cols * 3 + p2.x * 3 + 2] = 0;
										}
									}
								}
							}
							int oldPtsSize2 = ptsSize;
							for (int i = 0; i < d2; i++)
							{
								int ptsSizeTmp = ptsSize;
								i2 = 0;
								while (i2 < ptsSizeTmp)
								{
									pt p2 = pts[i2];
									i2++;
									if (p2.x > 0)
									{
										h2 = eptr[p2.y * cols * 3 + (p2.x - 1) * 3 + 0];
										s2 = eptr[p2.y * cols * 3 + (p2.x - 1) * 3 + 1];
										v2 = eptr[p2.y * cols * 3 + (p2.x - 1) * 3 + 2];
										int rh = abs(th2 - h2);
										if (rh > 90)rh = abs(rh - 180);
										if ((abs(ts2 - s2) < ds2) && (rh < dh2) && (abs(tv2 - v2) < dv2))
										{
											pts[ptsSize].x = p2.x - 1;
											pts[ptsSize].y = p2.y;
											ptsSize++;
											eptr[p2.y * cols * 3 + (p2.x - 1) * 3 + 0] = 0;
											eptr[p2.y * cols * 3 + (p2.x - 1) * 3 + 1] = 0;
											eptr[p2.y * cols * 3 + (p2.x - 1) * 3 + 2] = 0;
										}
									}
									if (p2.x < cols - 1)
									{
										h2 = eptr[p2.y * cols * 3 + (p2.x + 1) * 3 + 0];
										s2 = eptr[p2.y * cols * 3 + (p2.x + 1) * 3 + 1];
										v2 = eptr[p2.y * cols * 3 + (p2.x + 1) * 3 + 2];
										int rh = abs(th2 - h2);
										if (rh > 90)rh = abs(rh - 180);
										if ((abs(ts2 - s2) < ds2) && (rh < dh2) && (abs(tv2 - v2) < dv2))
										{
											pts[ptsSize].x = p2.x + 1;
											pts[ptsSize].y = p2.y;
											ptsSize++;
											eptr[p2.y * cols * 3 + (p2.x + 1) * 3 + 0] = 0;
											eptr[p2.y * cols * 3 + (p2.x + 1) * 3 + 1] = 0;
											eptr[p2.y * cols * 3 + (p2.x + 1) * 3 + 2] = 0;
										}
									}
									if (p2.y > 0)
									{
										h2 = eptr[(p2.y - 1) * cols * 3 + p2.x * 3 + 0];
										s2 = eptr[(p2.y - 1) * cols * 3 + p2.x * 3 + 1];
										v2 = eptr[(p2.y - 1) * cols * 3 + p2.x * 3 + 2];
										int rh = abs(th2 - h2);
										if (rh > 90)rh = abs(rh - 180);
										if ((abs(ts2 - s2) < ds2) && (rh < dh2) && (abs(tv2 - v2) < dv2))
										{
											pts[ptsSize].x = p2.x;
											pts[ptsSize].y = p2.y - 1;
											ptsSize++;
											eptr[(p2.y - 1) * cols * 3 + p2.x * 3 + 0] = 0;
											eptr[(p2.y - 1) * cols * 3 + p2.x * 3 + 1] = 0;
											eptr[(p2.y - 1) * cols * 3 + p2.x * 3 + 2] = 0;
										}
									}
									if (p2.y < rows - 1)
									{
										h2 = eptr[(p2.y + 1) * cols * 3 + p2.x * 3 + 0];
										s2 = eptr[(p2.y + 1) * cols * 3 + p2.x * 3 + 1];
										v2 = eptr[(p2.y + 1) * cols * 3 + (p2.x - 1) * 3 + 2];
										int rh = abs(th2 - h2);
										if (rh > 90)rh = abs(rh - 180);
										if ((abs(ts2 - s2) < ds2) && (rh < dh2) && (abs(tv2 - v2) < dv2))
										{
											pts[ptsSize].x = p2.x;
											pts[ptsSize].y = p2.y + 1;
											ptsSize++;
											eptr[(p2.y + 1) * cols * 3 + p2.x * 3 + 0] = 0;
											eptr[(p2.y + 1) * cols * 3 + p2.x * 3 + 1] = 0;
											eptr[(p2.y + 1) * cols * 3 + p2.x * 3 + 2] = 0;
										}
									}
								}
							}

							double radius = sqrt(oldPtsSize / 3.14);
							double expectedRed = 3.14 * ((radius + d1 + d2) * (radius + d1 + d2) - (radius + d1) * (radius + d1));
							int redPoint = ptsSize - oldPtsSize2;
							if (redPoint > expectedRed * 0.15) {
								point = 0;
								while (point < ptsSize)
								{
									ptr[pts[point].y * cols * 3 + pts[point].x * 3 + 0] = 255;
									ptr[pts[point].y * cols * 3 + pts[point].x * 3 + 1] = 0;
									ptr[pts[point].y * cols * 3 + pts[point].x * 3 + 2] = 0;
									point++;
								}
								if (Max_ptsSize < ptsSize)
								{
									Max_ptsSize = ptsSize;
									Max_ptsSize_coordinateX = (maxX + minX) / 2;
									Max_ptsSize_coordinateY = (maxY + minY) / 2;
								}
							}
						}
					}
				}
			}
		}
		//GaussianBlur(frame, edges, Size(7, 7), 1.5, 1.5);
		//Canny(edges, edges, 0, 30, 3);
		imshow("edges", frame);
		if (waitKey(30) >= 0) break;
	}
	// the camera will be deinitialized automatically in VideoCapture destructor
	return 0;
}
