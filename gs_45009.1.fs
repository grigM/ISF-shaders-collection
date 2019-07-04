/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#45009.1"
}
*/


//做出一个计时器, 时间范围从0.00秒到99.99秒
//1. 分别作出四位数字, 每一位数字的变化周期都是其低一位的数字变化周期的10.0倍
//2. 计时器中间的两个点, 两点以1.0秒为1周期闪烁
//3. 用矩形框架包裹计时器中间的数字区域
//precision mediump float;
float d_pot(vec2 point, float rad, float R)
{	//点函数
	return pow(rad / abs(length(point) - R), 2.0);
}
float d_seg(vec2 position, vec2 start_p, vec2 end_p, float R)
{	//线段函数
	vec2 AP = position - start_p;
	vec2 AB = end_p - start_p;
	float h = clamp(dot(AP, AB) / dot(AB, AB), 0.0, 1.0);
	float seg = abs(length(AP - AB * h) - R);
	return seg;
}
//与之前的点函数和线段函数不同, 坐标系中某一位置到点/线段的距离为R时, 函数返回值取最小值, 即点和线的亮度最大;
//当该距离大于或小于R时, 函数返回值均为一个大于0的值, 该值实际上等于某位置到半径为R的环/拉伸环的距离;
//因此点函数/线段函数并非确定了一个点/线段, 而是确定了一个半径为R的环/拉伸环.
float d_num(int t, vec2 position, float R)
{	//数字函数
	vec2 point[6];
	float seg[7], line[7], num[10];
	//数组point[6]为数字的各个端点, seg[7]为数字的各条边,
	//num[10]为数字0到9, 数组line[7]作减少计算量用
	//端点和边的分布如下所示, 用P0~P5表示point[5], 用L0~L5表示seg[6]
	//    P0--L0--P1
	//     |             |
	//    L5          L1
	//     |             |
	//    P5--L6--P2
	//     |             |
	//    L4          L2
	//     |             |
	//    P4--L3--P3
	point[0] = vec2(0.0, 0.5);
	point[1] = vec2(0.5, 0.5);
	point[2] = vec2(0.5, -0.0);
	point[3] = vec2(0.5, -0.5);
	point[4] = vec2(0.0, -0.5);
	point[5] = vec2(0.0, 0.0);
	seg[0] = d_seg(position, point[0], point[1], R);
	seg[1] = d_seg(position, point[1], point[2], R);
	seg[2] = d_seg(position, point[2], point[3], R);
	seg[3] = d_seg(position, point[3], point[4], R);
	seg[4] = d_seg(position, point[4], point[5], R);
	seg[5] = d_seg(position, point[5], point[0], R);
	seg[6] = d_seg(position, point[5], point[2], R);
	for(int i = 0; i < 7; i++)
		line[i] = 0.0002 / (seg[i] * seg[i]);
	num[0] = line[0] + line[1] + line[2] + line[3] + line[4] + line[5];
	num[1] = line[1] + line[2];
	num[2] = line[0] + line[1] + line[3] + line[4] + line[6];
	num[3] = line[0] + line[1] + line[2] + line[3] + line[6];
	num[4] = line[1] + line[2] + line[5] + line[6];
	num[5] = line[0] + line[2] + line[3] + line[5] + line[6];
	num[6] = line[0] + line[2] + line[3] + line[4] + line[5] + line[6];
	num[7] = line[0] + line[1] + line[2];
	num[8] = line[0] + line[1] + line[2] + line[3] + line[4] + line[5] + line[6];
	num[9] = line[0] + line[1] + line[2] + line[3] + line[5] + line[6];
	//下面的条件语句相当于return num[int(t)],
	//由于glsl中数组下标必须为const int型数据, 故用条件语句代替
	#define getnum(a) if(t == a) return num[a]
	getnum(0);
	getnum(1);
	getnum(2);
	getnum(3);
	getnum(4);
	getnum(5);
	getnum(6);
	getnum(7);
	getnum(8);
	getnum(9);
}
void main( void ) {
	vec2 position = (( gl_FragCoord.xy / RENDERSIZE.xy ) - vec2(0.5,0.5)) * 5.0;
	position.x = position.x*(RENDERSIZE.x / RENDERSIZE.y) - 0.5;
	vec4 color = vec4(2.0 * abs(fract(0.1 * TIME) - 0.5), 0.3, 1.0, 1.0);	//数字, 框架和圆点的颜色
	float number[6], line[4], light = 0.0, cycle, R = 0.05;
	//number[0]到number[3]为计时器数字, 数组序号越大, 显示位越高;
	//number[4]和number[5]为计时器中间两个点;
	//数组line[4]为计时器外围矩形框架的四条边, cycle为计时器中间两点的显示周期
	
	//1. 分别作出四位数字, 每一位数字的变化周期都是其低一位的数字变化周期的10.0倍
	number[0] = d_num(int(10.0 * fract(10.0 * TIME)), position - vec2(1.5, 0.0), R);
	number[1] = d_num(int(10.0 * fract(1.0 * TIME)), position - vec2(0.5, 0.0), R);
	number[2] = d_num(int(10.0 * fract(0.1 * TIME)), position - vec2(-1.0, 0.0), R);
	number[3] = d_num(int(10.0 * fract(0.01 * TIME)), position - vec2(-2.0, 0.0), R);
	//2. 计时器中间的两个点, 两点以1.0秒为1周期闪烁
	cycle = step(fract(TIME), fract(TIME + 0.5));
	//step(a, b)为阶梯比较函数如果(b<a), 返回0.0, 否则返回1.0, 这里有一半时间cycle的值为0.0, 其余为1.0
	number[4] = cycle * d_pot(vec2(0.0, -0.25) + position, 0.02, R);
	number[5] = cycle * d_pot(vec2(0.0, +0.25) + position, 0.02, R);
	for(int i = 0; i < 6; i++)		//叠加数字和两个点的亮度
		light += number[i];
	//3. 用矩形框架包裹中间的数字区域
	line[0] = d_seg(position, vec2(-2.5, -1.0), vec2(2.5, -1.0), R);
	line[1] = d_seg(position, vec2(2.5, -1.0), vec2(2.5, 1.0), R);
	line[2] = d_seg(position, vec2(2.5, 1.0), vec2(-2.5, 1.0), R);
	line[3] = d_seg(position, vec2(-2.5, 1.0), vec2(-2.5, -1.0), R);
	for(int i = 0; i < 4; i++)		//叠加外围框架的亮度
		light += 0.001 / (line[i] * line[i]);
	gl_FragColor = clamp(color * light, 0.0, 1.0);
}