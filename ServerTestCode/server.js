const express = require('express');
const http = require('http');
const socketIo = require('socket.io');

const app = express();
const server = http.createServer(app);
const io = socketIo(server);

let players = {}; // 플레이어 정보를 저장할 객체

io.on('connection', (socket) => {
    console.log('클라이언트 연결됨:', socket.id);

    // 새 플레이어가 연결될 때
    players[socket.id] = {
        id: socket.id,
        position: { 'x' : 0 , 'y': 0, 'z': 0 }
    };

    // 클라이언트에게 현재 플레이어 목록 전송
    socket.emit('currentPlayers', players);

    // 다른 클라이언트에게 새 플레이어 정보 전송
    socket.broadcast.emit('newPlayer', players[socket.id]);

    socket.on('playerMovement', (movementData) => {
        // 플레이어의 움직임을 업데이트
        players[socket.id].position = movementData.position;
        console.log(players[socket.id].position);
        // 다른 클라이언트에게 플레이어 움직임 정보 전송
        io.emit('playerMoved', players[socket.id]);
    });

    socket.on('disconnect', () => {
        console.log('클라이언트 연결 끊김:', socket.id);
        delete players[socket.id]; // 연결이 끊어진 플레이어 삭제
        socket.broadcast.emit('playerDisconnected', socket.id); // 다른 클라이언트에게 알림
    });
});

const PORT = 3333;
server.listen(PORT, () => {
    console.log(`서버가 http://localhost:${PORT} 에서 실행 중입니다.`);
});
