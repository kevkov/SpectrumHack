import React from 'react';
import {Card, CardItem, Content, Text, Left, Right, Body} from "native-base";
import { FlatList, StyleSheet } from 'react-native';

const routeInfoItems = [
    {
        key: "Route A",
        routeLabel: "Route A",
        colorInHex: "#ff0000",
        pollutionPoint: 5,
        travelTime: "01:27:00",
        travelCost: 1.23,
        schoolCount: 4,
        distance: 12.4
    },
    {
        key: "Route B",
        routeLabel: "Route B",
        colorInHex: "#00ff00",
        pollutionPoint: 5,
        travelTime: "01:27:00",
        travelCost: 1.23,
        schoolCount: 4,
        distance: 11.9
     },
     {
        key: "Route C",
        routeLabel: "Route C",
        colorInHex: "#0000ff",
        pollutionPoint: 5,
        travelTime: "01:27:00",
        travelCost: 1.23,
        schoolCount: 4,
        distance: 12.8
     }];

const GetHeaderStyle = (backgroundColourHex: string) => {
    return { 
        backgroundColor: backgroundColourHex,
    };
}

const styles = StyleSheet.create({
    headerText: {
        fontWeight: '600'
    },
    content: {
        padding: 10
    }
});

export const JourneyDetails = (props) => {

    return (
        <Content style={styles.content}>
            <FlatList
                    data={routeInfoItems}
                    renderItem={datum =>                    
            <Card>
                <CardItem bordered style={GetHeaderStyle(datum.item.colorInHex)}>
                    <Text style={styles.headerText}>{datum.item.routeLabel}</Text>
                </CardItem>
                <CardItem>
                    <Left>
                        <Text>Air Quality Index: {datum.item.pollutionPoint}</Text>
                    </Left>
                    <Body>
                        <Text>Schools: {datum.item.schoolCount}</Text>
                    </Body>
                </CardItem>
                <CardItem>
                    <Left>
                        <Text>Distance: {datum.item.distance}m</Text>
                    </Left>
                    <Body>
                        <Text>Travel Time: {datum.item.travelTime}</Text>
                    </Body>
                </CardItem>
                <CardItem>
                    <Left>
                        <Text style={styles.headerText}>Travel Cost: {datum.item.travelCost}</Text>
                    </Left>
                </CardItem>
            </Card>
            }/>
        </Content>
    )
};