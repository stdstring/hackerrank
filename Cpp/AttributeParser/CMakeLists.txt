cmake_minimum_required(VERSION 3.6 FATAL_ERROR)

project(AttributeParser CXX)

set(CMAKE_CXX_STANDARD 11)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

add_executable(AttributeParser AttributeParser.cpp)